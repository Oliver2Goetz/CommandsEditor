using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CommandsEditor.DockPanels {

    internal static class DockPanelsSettings {

        public static readonly Color BACKGROUND_COLOR = Color.FromArgb(30, 30, 30);
        public static readonly Color FOREGROUND_COLOR = Color.WhiteSmoke;
        public static readonly Color ACCENT_COLOR = Color.FromArgb(200, 200, 200);

        public static void ApplyTheme(
            Control root,
            ToolStrip[] toolStrips = null,
            ContextMenuStrip[] contextMenus = null
        ) {
            if (root == null) {
                return;
            }

            root.BackColor = BACKGROUND_COLOR;
            root.ForeColor = FOREGROUND_COLOR;

            // Theme the existing tree
            ApplyRecursive(root.Controls);

            // Theme explicitly passed strips/menus (they live in components, not Controls)
            if (toolStrips != null) { 
                foreach (var ts in toolStrips.Where(s => s != null)) { 
                    ApplyToolStrip(ts);
                }
            }

            if (contextMenus != null) { 
                foreach (var cms in contextMenus.Where(m => m != null)) {
                    ApplyContextMenu(cms);
                }
            }

            // Also theme any ToolStrip that actually is in Controls
            foreach (var ts in root.Controls.OfType<ToolStrip>()) {
                ApplyToolStrip(ts);
            }

            // Theme future controls created at runtime
            root.ControlAdded -= Root_ControlAdded;
            root.ControlAdded += Root_ControlAdded;

            // Global renderer (helps drop-downs not passed explicitly)
            ToolStripManager.Renderer = new ToolStripProfessionalRenderer(new SimpleColorTable());
        }

        private static void Root_ControlAdded(object sender, ControlEventArgs e) {
            ApplyToControl(e.Control);
            if (e.Control.HasChildren) {
                ApplyRecursive(e.Control.Controls);
            }
        }

        private static void ApplyRecursive(Control.ControlCollection controls) {
            foreach (Control c in controls) { 
                ApplyToControl(c);
                if (c.HasChildren) {
                    ApplyRecursive(c.Controls);
                }
            }
        }

        private static void ApplyToControl(Control c) {
            c.ForeColor = FOREGROUND_COLOR;

            if (c is ContextMenuStrip cms) {
                ApplyContextMenu(cms);
                return;
            }

            if (c is ToolStrip ts) {
                ApplyToolStrip(ts);
                return;
            }

            if (c is Button b) {
                ApplyButton(b);
                return;
            }

            if (c is TextBox tb) {
                tb.BackColor = Color.FromArgb(45, 45, 45);
                tb.ForeColor = FOREGROUND_COLOR;
                return;
            }

            if (c is GroupBox gb) {
                gb.BackColor = BACKGROUND_COLOR;
                gb.ForeColor = FOREGROUND_COLOR;
                return;
            }

            if (c is TableLayoutPanel tlp) {
                tlp.BackColor = BACKGROUND_COLOR;
                return;
            }

            if (c is Panel p) {
                p.BackColor = BACKGROUND_COLOR;
                return;
            }

            // default
            c.BackColor = BACKGROUND_COLOR;
        }

        private static void ApplyButton(Button b) {
            b.UseVisualStyleBackColor = false;
            b.FlatStyle = FlatStyle.Flat;
            b.BackColor = BACKGROUND_COLOR;
            b.ForeColor = FOREGROUND_COLOR;

            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.BorderColor = Color.FromArgb(200,200,200);
            b.FlatAppearance.MouseOverBackColor = Color.FromArgb(45,45,45);
            b.FlatAppearance.MouseDownBackColor = Color.FromArgb(65,65,65);
        }

        private static void ApplyToolStrip(ToolStrip ts) {
            ts.RenderMode = ToolStripRenderMode.Professional;
            ts.Renderer = new ToolStripProfessionalRenderer(new SimpleColorTable());
            ts.BackColor = BACKGROUND_COLOR;
            ts.ForeColor = FOREGROUND_COLOR;

            foreach (ToolStripItem i in ts.Items) { 
                i.ForeColor = FOREGROUND_COLOR;
            }
        }

        private static void ApplyContextMenu(ContextMenuStrip cms) {
            cms.RenderMode = ToolStripRenderMode.Professional;
            cms.Renderer = new ToolStripProfessionalRenderer(new SimpleColorTable());
            cms.BackColor = BACKGROUND_COLOR;
            cms.ForeColor = FOREGROUND_COLOR;

            foreach (ToolStripItem i in cms.Items) { 
                i.ForeColor = FOREGROUND_COLOR;
            }
        }

        private sealed class SimpleColorTable : ProfessionalColorTable {
            public SimpleColorTable() {
                this.UseSystemColors = false;
            }

            public override Color ToolStripGradientBegin => BACKGROUND_COLOR;
            public override Color ToolStripGradientMiddle => BACKGROUND_COLOR;
            public override Color ToolStripGradientEnd => BACKGROUND_COLOR;
            public override Color MenuItemSelected => Color.FromArgb(60, 60, 60);
            public override Color MenuBorder => Color.FromArgb(70, 70, 70);
            public override Color ButtonSelectedBorder => ACCENT_COLOR;
        }
    }
}