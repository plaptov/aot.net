using Aot.Net.MorphDict.Common;

namespace Aot.Net.MorphDict.LemmatizerBaseLib
{
	public record struct PredictTuple(ushort ItemNo, int LemmaInfoNo, byte PartOfSpeechNo);

	public class PredictBase
	{
		private readonly MorphAutomat _suffixAutomat;
		private readonly char[] _chars;
		private uint[] _modelFreq;

		public PredictBase(MorphLanguage language)
		{
			_suffixAutomat = new MorphAutomat(language, CABCEncoder.MorphAnnotChar);
			_chars = Encodings.GetNonUnicodeChars(language);
		}

		public IReadOnlyList<uint> ModelFreq => _modelFreq;

		/// <summary>
		/// </summary>
		/// <param name="predictFile"><c>npredict.bin</c> file</param>
		public void Load(Stream predictFile)
		{
			_suffixAutomat.Load(predictFile);
		}

		public void FillModelFreq(int flexiaModelsCount, IEnumerable<LemmaInfoAndLemma> lemmaInfos)
		{
			_modelFreq = new uint[flexiaModelsCount];
			foreach (var item in lemmaInfos)
				_modelFreq[item.LemmaInfo.FlexiaModelNo]++;
		}

		public bool Find(ReadOnlySpan<char> reversedWordForm, out List<PredictTuple> res)
		{
			//  we don't want to predict words which contains "AnnotChar" 
			//if (ReversedWordForm.find(AnnotChar) != std::string::npos)
			//	return false;

			var TextLength = reversedWordForm.Length;
			int r = 0;
			int i = 0;
			for (; i < TextLength; i++)
			{
				int nd = _suffixAutomat.NextNode(r, reversedWordForm[i]);

				if (nd == -1)
					break;
				r = nd;
			}
			res = new List<PredictTuple>();
			// no prediction by suffix which is less than 3
			if (i < CABCEncoder.MinimalPredictionSuffix)
				return false;

			if (r == -1)
				throw new Exception();
			Span<char> path = stackalloc char[32];
			Span<PredictTuple> buf = stackalloc PredictTuple[4];
			var smallList = new SmallList<PredictTuple>(buf);
			FindRecursive(r, path, 0, ref smallList);
			res = smallList.ToList();
			return true;
		}

		private void FindRecursive(
			int nodeNo,
			Span<char> curr_path,
			int curr_len,
			ref SmallList<PredictTuple> infos)
		{
			var N = _suffixAutomat.Nodes[nodeNo];
			if (N.IsFinal())
			{
				var i = curr_path.IndexOf(_suffixAutomat.AnnotChar);
				if (i < 0)
					throw new Exception();
				var j = curr_path[(i+1)..].IndexOf(_suffixAutomat.AnnotChar) + i + 1;
				if (j < 0)
					throw new Exception();
				var k = curr_path[(j+1)..].IndexOf(_suffixAutomat.AnnotChar) + j + 1;
				if (k < 0)
					throw new Exception();
				var partOfSpeechNo = _suffixAutomat.DecodeFromAlphabet(curr_path[(i + 1)..j]);
				var lemmaInfoNo = _suffixAutomat.DecodeFromAlphabet(curr_path[(j + 1)..k]);
				var itemNo = _suffixAutomat.DecodeFromAlphabet(curr_path[(k + 1)..curr_len]);
				infos.Add(new((ushort)itemNo, lemmaInfoNo, (byte)partOfSpeechNo));
			}

			var count = _suffixAutomat.GetChildrenCount(nodeNo);
			for (int i = 0; i < count; i++)
			{
				var p = _suffixAutomat.GetChildren(nodeNo, i);
				curr_path[curr_len] = _chars[p.GetRelationalChar()];
				FindRecursive(p.GetChildNo(), curr_path, curr_len + 1, ref infos);
			}
		}
	}
}
