using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;

namespace MFAX01V3
{
	public class HighlightedTextBlock : TextBlock
	{
		public string Highlight
		{
			get { return (string)this.GetValue(HighlightProperty); }
			set { this.SetValue(HighlightProperty, value); }
		}
		public static readonly DependencyProperty HighlightProperty = DependencyProperty.Register(
		  "Highlight", typeof(string), typeof(HighlightedTextBlock), new PropertyMetadata(string.Empty, OnTextOrHighlightChanged));

		public string RawText
		{
			get { return (string)this.GetValue(RawTextProperty); }
			set { this.SetValue(RawTextProperty, value); }
		}
		public static readonly DependencyProperty RawTextProperty = DependencyProperty.Register(
		  "RawText", typeof(string), typeof(HighlightedTextBlock), new PropertyMetadata(string.Empty, OnTextOrHighlightChanged));


		static HighlightedTextBlock()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(HighlightedTextBlock), new FrameworkPropertyMetadata(typeof(HighlightedTextBlock)));
		}

		private static void OnTextOrHighlightChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			HighlightedTextBlock textBlock = (HighlightedTextBlock)obj;
			textBlock.Inlines.Clear();

			string _matchPre = string.Empty;
			string _match = string.Empty;
			string _matchPost = string.Empty;
			string _highlight = textBlock.Highlight;
			string _text = textBlock.RawText; // textBlock.Text;
			int index = -1;

			if (_highlight != null && _text != null)
				index = _text.IndexOf(_highlight, StringComparison.CurrentCultureIgnoreCase);

			if (index < 0)
				_matchPre = _text;
			else
			{
				_matchPre = _text.Substring(0, index);
				_match = _text.Substring(index, _highlight.Length);
				_matchPost = _text.Substring(index + _highlight.Length);
			}

			if (_matchPre.Length > 0)
				textBlock.Inlines.Add(new Run(_matchPre));
			if (_match.Length > 0)
				textBlock.Inlines.Add(new Run(_match) { FontWeight = FontWeights.Bold });
			if (_matchPost.Length > 0)
				textBlock.Inlines.Add(new Run(_matchPost));
		}

	} //HighlightedTextBlock
}
