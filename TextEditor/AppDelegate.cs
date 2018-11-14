using AppKit;
using Foundation;
using System.IO;

namespace TextEditor
{
    [Register("AppDelegate")]
    public class AppDelegate : NSApplicationDelegate
    {
        public int UntitledWindowCount { get; set; } = 1;
        public AppDelegate()
        {
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            // Insert code here to initialize your application
        }
        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
        [Export("newDocument:")]
        void Newdocument(NSObject sender)
        {
            var storyboard = NSStoryboard.FromName("Main", null);
            var controller = storyboard.InstantiateControllerWithIdentifier("MainWindow") as NSWindowController;
            controller.ShowWindow(this);
            controller.Window.Title = (++UntitledWindowCount == 1) ? "untitled" : string.Format("untitle {0}", UntitledWindowCount);
        }

        [Export("openDocument:")]
        void OpenDialog(NSObject sender)
        {
            var dlg = NSOpenPanel.OpenPanel;
            dlg.CanChooseFiles = false;
            dlg.CanChooseDirectories = true;

            if (dlg.RunModal() == 1)
            {
                var alert = new NSAlert()
                {
                    AlertStyle = NSAlertStyle.Informational,
                    InformativeText = "At this point we should do something with the folder that the user just selected in the Open File Dialog box...",
                    MessageText = "Folder Selected"
                };
                alert.RunModal();
            }
        }
       
    }
}
