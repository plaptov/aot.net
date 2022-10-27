using Aot.Net.MorphDict.Common;

namespace Aot.Net.MorphDict.LemmatizerBaseLib
{
	public class MorphAutomat : CABCEncoder
	{
		private readonly char[] _chars;
		private int[] _childrenCache;

		public MorphAutomat(MorphLanguage language, char annotChar) : base(language, annotChar)
		{
			_chars = Encodings.GetNonUnicodeChars(language);
		}

		public IReadOnlyList<MorphAutomNode> Nodes { get; private set; }

		public IReadOnlyList<MorphAutomRelation> Relations { get; private set; }

		public void Load(Stream stream)
		{
			using (var reader = new BinaryReader(stream))
			{
				var s = reader.ReadTerminatedShortString();
				var nodesCount = int.Parse(s);
				var nodes = new MorphAutomNode[nodesCount];
				for (int i = 0; i < nodesCount; i++)
					nodes[i] = new MorphAutomNode(reader.ReadUInt32());
				Nodes = nodes;

				s = reader.ReadTerminatedShortString();
				var relationsCount = int.Parse(s);
				var relations = new MorphAutomRelation[relationsCount];
				for (int i = 0; i < relationsCount; i++)
					relations[i] = new MorphAutomRelation(reader.ReadUInt32());
				Relations = relations;

				var chars = Encodings.GetNonUnicodeChars(Language);
				foreach (var c in chars)
				{
					if (reader.ReadInt32() != Alphabet2Code[c])
						throw new InvalidOperationException($"{Language} alphabet has changed; cannot load morph automat");
				}
			}
			BuildChildrenCache();
		}

		public int GetChildrenCount(int NodeNo)
		{
			if (NodeNo + 1 == Nodes.Count)
				return Relations.Count - Nodes[NodeNo].GetChildrenStart();
			else
				return Nodes[NodeNo + 1].GetChildrenStart() - Nodes[NodeNo].GetChildrenStart();
		}

		private void BuildChildrenCache()
		{
			var Count = ChildrenCacheSize;
			if (Nodes.Count < ChildrenCacheSize)
				Count = Nodes.Count;

			_childrenCache = new int[Count * MaxAlphabetSize];

			for (var NodeNo = 0; NodeNo < Count; NodeNo++)
			{
				var start = Nodes[NodeNo].GetChildrenStart();
				var end = start + GetChildrenCount(NodeNo);
				for (; start != end; start++)
				{
					MorphAutomRelation p = Relations[start];
					_childrenCache[NodeNo * MaxAlphabetSize + Alphabet2Code[_chars[p.GetRelationalChar()]]] = p.GetChildNo();
				};
			};
		}

		internal int NextNode(int NodeNo, char RelationChar)
		{
			if (NodeNo < ChildrenCacheSize)
			{
				int z = Alphabet2Code[RelationChar];
				if (z == -1)
					return -1;
				return _childrenCache[NodeNo * MaxAlphabetSize + z];
			}
			else
			{
				var start = Nodes[NodeNo].GetChildrenStart();
				var end = start + GetChildrenCount(NodeNo);

				for (; start != end; start++)
				{
					MorphAutomRelation p = Relations[start];
					if (RelationChar == _chars[p.GetRelationalChar()])
						return p.GetChildNo();
				};

				return -1;
			};
		}

		public MorphAutomRelation GetChildren(int NodeNo, int childNo = 0)
		{
			return Relations[Nodes[NodeNo].GetChildrenStart() + childNo];
		}

		private int FindStringAndPassAnnotChar(string Text, int TextPos)
		{
			var TextLength = Text.Length;
			int r = 0;
			for (var i = TextPos; i < TextLength; i++)
			{
				int nd = NextNode(r, Text[i]);

				if (nd == -1)
				{
					return -1;
				}
				r = nd;
			};
			//assert ( r != -1);
			//  passing annotation char

			return NextNode(r, AnnotChar);
		}

		public static void DecodeMorphAutomatInfo(int Info, out int ModelNo, out ushort ItemNo, out ushort PrefixNo)
		{
			ModelNo = Info >> 18;
			ItemNo = (ushort)((0x3FFFF & Info) >> 9);
			PrefixNo = (ushort)(0x1FF & Info);
		}

		private void GetAllMorphInterpsRecursive(int NodeNo, List<char> curr_path, List<AutomAnnotationInner> Infos)
		{
			MorphAutomNode N = Nodes[NodeNo];
			if (N.IsFinal())
			{
				int i = DecodeFromAlphabet(curr_path);
				DecodeMorphAutomatInfo(i, out var ModelNo, out var ItemNo, out var PrefixNo);
				AutomAnnotationInner A = new(ModelNo, ItemNo, PrefixNo);
				Infos.Add(A);
			};

			int Count = GetChildrenCount(NodeNo);
			int CurrPathSize = curr_path.Count;
			curr_path.Add(default);
			for (int i = 0; i < Count; i++)
			{
				MorphAutomRelation p = GetChildren(NodeNo, i);
				curr_path[CurrPathSize] = _chars[p.GetRelationalChar()];
				GetAllMorphInterpsRecursive(p.GetChildNo(), curr_path, Infos);
			};
			curr_path.RemoveRange(CurrPathSize, curr_path.Count - CurrPathSize);
		}

		public List<AutomAnnotationInner> GetInnerMorphInfos(string text, int textPos)
		{
			var Infos = new List<AutomAnnotationInner>();
			int r = FindStringAndPassAnnotChar(text, textPos);
			if (r == -1)
				return Infos;
			GetAllMorphInterpsRecursive(r, new List<char>(), Infos);
			return Infos;
		}
	}
}
