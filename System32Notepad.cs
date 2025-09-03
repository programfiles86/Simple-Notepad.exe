using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace System32Notepad
{
    public class Notepad : Form
    {
        private RichTextBox textArea;
        private MenuStrip menu;
        private string currentFile = "";

        public Notepad()
        {
            this.Text = "System32 Notepad";
            this.Width = 800;
            this.Height = 600;

            textArea = new RichTextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 12),
                BackColor = Color.Black,
                ForeColor = Color.Lime
            };
            this.Controls.Add(textArea);

            menu = new MenuStrip();
            this.MainMenuStrip = menu;
            this.Controls.Add(menu);

            // File menu
            var fileMenu = new ToolStripMenuItem("File");
            fileMenu.DropDownItems.Add("New", null, (s, e) => { textArea.Clear(); currentFile = ""; });
            fileMenu.DropDownItems.Add("Open", null, OpenFile);
            fileMenu.DropDownItems.Add("Save", null, SaveFile);
            fileMenu.DropDownItems.Add("Save As", null, SaveFileAs);
            fileMenu.DropDownItems.Add("Exit", null, (s, e) => this.Close());
            menu.Items.Add(fileMenu);

            // Edit menu
            var editMenu = new ToolStripMenuItem("Edit");
            editMenu.DropDownItems.Add("Cut", null, (s, e) => textArea.Cut());
            editMenu.DropDownItems.Add("Copy", null, (s, e) => textArea.Copy());
            editMenu.DropDownItems.Add("Paste", null, (s, e) => textArea.Paste());
            menu.Items.Add(editMenu);

            // Format menu
            var formatMenu = new ToolStripMenuItem("Format");
            formatMenu.DropDownItems.Add("Change Font", null, ChangeFont);
            formatMenu.DropDownItems.Add("Change Colors", null, ChangeColors);
            menu.Items.Add(formatMenu);

            // Help menu
            var helpMenu = new ToolStripMenuItem("Help");
            helpMenu.DropDownItems.Add("About", null, (s, e) =>
            {
                MessageBox.Show("System32 Notepad\nCreated by System32\n2025", "About");
            });
            menu.Items.Add(helpMenu);
        }

        private void OpenFile(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Text Files|*.txt|All Files|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textArea.Text = File.ReadAllText(dialog.FileName);
                currentFile = dialog.FileName;
            }
        }

        private void SaveFile(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFile))
                SaveFileAs(sender, e);
            else
                File.WriteAllText(currentFile, textArea.Text);
        }

        private void SaveFileAs(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "Text Files|*.txt|All Files|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(dialog.FileName, textArea.Text);
                currentFile = dialog.FileName;
            }
        }

        private void ChangeFont(object sender, EventArgs e)
        {
            var dialog = new FontDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
                textArea.Font = dialog.Font;
        }

        private void ChangeColors(object sender, EventArgs e)
        {
            var dialog = new ColorDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
                textArea.ForeColor = dialog.Color;

            dialog = new ColorDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
                textArea.BackColor = dialog.Color;
        }

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new Notepad());
        }
    }
}
