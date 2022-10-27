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

		public bool Find(string reversedWordForm, out List<PredictTuple> res)
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
			};
			res = new List<PredictTuple>();
			// no prediction by suffix which is less than 3
			if (i < CABCEncoder.MinimalPredictionSuffix)
				return false;

			if (r == -1)
				throw new Exception();
			FindRecursive(r, new List<char>(), res);
			return true;
		}

		private void FindRecursive(int nodeNo, List<char> curr_path, List<PredictTuple> infos)
		{
			var N = _suffixAutomat.Nodes[nodeNo];
			if (N.IsFinal())
			{
				var i = curr_path.IndexOf(_suffixAutomat.AnnotChar);
				if (i < 0)
					throw new Exception();
				var j = curr_path.IndexOf(_suffixAutomat.AnnotChar, i + 1);
				if (j < 0)
					throw new Exception();
				var k = curr_path.IndexOf(_suffixAutomat.AnnotChar, j + 1);
				if (k < 0)
					throw new Exception();
				var partOfSpeechNo = _suffixAutomat.DecodeFromAlphabet(curr_path.Skip(i).Take(j - i - 1));
				var lemmaInfoNo = _suffixAutomat.DecodeFromAlphabet(curr_path.Skip(j).Take(k - j - 1));
				var itemNo = _suffixAutomat.DecodeFromAlphabet(curr_path.Skip(k));
				infos.Add(new((ushort)itemNo, lemmaInfoNo, (byte)partOfSpeechNo));
			}

			var count = _suffixAutomat.GetChildrenCount(nodeNo);
			int currPathSize = curr_path.Count;
			curr_path.Add(default);
			for (int i = 0; i < count; i++)
			{
				var p = _suffixAutomat.GetChildren(nodeNo, i);
				curr_path[currPathSize] = _chars[p.GetRelationalChar()];
				FindRecursive(p.GetChildNo(), curr_path, infos);
			}
			curr_path.RemoveRange(currPathSize, curr_path.Count - currPathSize);
		}
	}
}
