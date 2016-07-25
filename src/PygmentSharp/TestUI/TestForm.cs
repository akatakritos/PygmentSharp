using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using PygmentSharp.Core;
using PygmentSharp.Core.Formatters;

namespace TestUI
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                var filename = dialog.FileName;

                var locator = new LexerLocator();
                var lexer = locator.FindByFilename(filename);

                if (lexer == null)
                {
                    MessageBox.Show(@"I couldn't find a lexer for this file", @"No Lexer Available", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var tokens = lexer.GetTokens(File.ReadAllText(filename)).ToArray();

                var formatter = new HtmlFormatter(new HtmlFormatterOptions() {Full = true});
                var buffer = new StringWriter();
                formatter.Format(tokens, buffer);

                ShowHtml(buffer.ToString());
                ShowTokens(tokens);
            }
        }

        private void ShowTokens(IEnumerable<Token> tokens)
        {
            tokenList.Items.Clear();
            tokenList.BeginUpdate();

            try
            {
                foreach (var token in tokens)
                {
                    var lvi = new ListViewItem(token.Index.ToString());
                    lvi.SubItems.Add(token.Type.ToString());
                    lvi.SubItems.Add(token.Value);

                    tokenList.Items.Add(lvi);
                }
            }
            finally
            {
                tokenList.EndUpdate();
            }
        }

        private void ShowHtml(string html)
        {
            browser.DocumentText = "0";
            browser.Document?.OpenNew(true);
            browser.Document?.Write(html);
            browser.Refresh();
        }
    }
}
