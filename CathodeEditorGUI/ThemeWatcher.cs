using CommandsEditor.DockPanels;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CommandsEditor {

    internal static class ThemeWatcher {

        private static bool _started;

        public static void Start() {
            if (_started) {
                return;
            }
            _started = true;

            // timer: check for new forms every 500ms. Probably the worst option to do this but whatever
            var t = new Timer { Interval = 500 };
            t.Tick += (s, e) => {
                foreach (Form f in Application.OpenForms) {
                    if (f.Tag as string == "themed") {
                        continue;
                    }

                    f.Tag = "themed"; // mark as done
                    try {
                        DockPanelsSettings.ApplyTheme(f);

                        var toolStrips = f.Controls.OfType<ToolStrip>().ToArray();
                        var menus = f.Controls.OfType<ContextMenuStrip>().ToArray();
                        if (toolStrips.Length > 0 || menus.Length > 0)
                            DockPanelsSettings.ApplyTheme(f, toolStrips, menus);
                    } catch { /* ignore */ }
                }
            };
            t.Start();
        }
    }
}