namespace Aot.Net.MorphDict.LemmatizerBaseLib
{
	public class ImmutablePrefixTree
	{
		private readonly Node[] _nodes;

		public ImmutablePrefixTree()
		{
			_nodes = new[] { new Node() };
		}

		public ImmutablePrefixTree(IEnumerable<string> strings)
		{
			var rootNode = new TempNode('\0');
			foreach (var s in strings)
				AddString(rootNode, s);
			var nodes = new List<Node>();
			nodes.Add(new Node(rootNode.Character, (byte)rootNode.Children.Count, 1));
			MapChildren(rootNode, nodes);
			_nodes = nodes.ToArray();
		}

		public bool ContainsString(ReadOnlySpan<char> s)
		{
			ref var node = ref _nodes[0];
			foreach (var c in s)
			{
				node = ref TryGetChild(node, c);
				if (node.Character == '\0')
					return false;
			}
			return node.IsFinal();
		}

		private ref Node TryGetChild(Node parent, char c)
		{
			for (int i = parent.ChildrenStart; i < parent.ChildrenStart + parent.ChildrenCount; i++)
			{
				ref var node = ref _nodes[i];
				if (node.Character == c)
					return ref node;
			}
			return ref _nodes[0];
		}

		private static void MapChildren(TempNode node, List<Node> nodes)
		{
			int childrenStart = nodes.Count + node.Children.Count;
			foreach (var child in node.Children)
			{
				nodes.Add(new Node(
					child.Character,
					(byte)child.Children.Count,
					childrenStart
				));
				childrenStart += child.RecursiveChildrenCount();
			}
			foreach (var child in node.Children)
				MapChildren(child, nodes);
		}

		private static void AddString(TempNode node, string s)
		{
			foreach (var c in s)
				node = node.GetOrAddChild(c);
		}

		private readonly struct Node
		{
			public readonly char Character;
			public readonly byte ChildrenCount;
			public readonly int ChildrenStart;

			public Node(char character, byte childrenCount, int childrenStart)
			{
				Character = character;
				ChildrenCount = childrenCount;
				ChildrenStart = childrenStart;
			}

			public bool IsFinal() => ChildrenCount == 0;
		}

		private class TempNode
		{
			public TempNode(char character)
			{
				Character = character;
				Children = new();
			}

			public char Character { get; }
			public List<TempNode> Children { get; }

			public TempNode GetOrAddChild(char c)
			{
				foreach (var item in Children)
				{
					if (item.Character == c)
						return item;
				}
				var child = new TempNode(c);
				Children.Add(child);
				return child;
			}

			public int RecursiveChildrenCount() =>
				Children.Count + Children.Sum(ch => ch.RecursiveChildrenCount());
		}
	}
}
