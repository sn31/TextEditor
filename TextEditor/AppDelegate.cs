using AppKit;
using Foundation;
using System.IO;
using System;

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

        }
        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
       
        [Export("newDocument:")]
        void NewDocument(NSObject sender)
        {
            // Get new window
            var storyboard = NSStoryboard.FromName("Main", null);
            var controller = storyboard.InstantiateControllerWithIdentifier("MainWindow") as NSWindowController;

            // Display
            controller.ShowWindow(this);
            Console.WriteLine(UntitledWindowCount);
            // Set the title
            controller.Window.Title = (++UntitledWindowCount == 1) ? "untitled" : string.Format("untitled {0}", UntitledWindowCount);
            Console.WriteLine(UntitledWindowCount);

        }
        private bool OpenFile(NSUrl url)
        {
            var good = false;

            // Trap all errors
            try
            {
                var path = url.Path;

                //Is the file already open?
                foreach (NSWindow window in NSApplication.SharedApplication.Windows)
                {
                    if (window.ContentViewController is ViewController content && path == content.FilePath)
                    {
                        // Bring window to front
                        window.MakeKeyAndOrderFront(this);
                        return true;
                    }
                }

                // Get new window
                var storyboard = NSStoryboard.FromName("Main", null);
                var controller = storyboard.InstantiateControllerWithIdentifier("MainWindow") as NSWindowController;

                // Display
                controller.ShowWindow(this);

                // Load the text into the window
                var viewController = controller.Window.ContentViewController as ViewController;
                viewController.Text = File.ReadAllText(path);
                viewController.View.Window.SetTitleWithRepresentedFilename(Path.GetFileName(path));
                viewController.View.Window.RepresentedUrl = url;

                // Add document to the Open Recent menu
                NSDocumentController.SharedDocumentController.NoteNewRecentDocumentURL(url);

                // Make as successful
                good = true;
            }
            catch
            {
                // Mark as bad file on error
                good = false;
            }

            // Return results
            return good;
        }

        [Export("openDocument:")]
        void OpenDialog(NSObject sender)
        {

            var dlg = NSOpenPanel.OpenPanel;
            dlg.CanChooseFiles = true;
            dlg.CanChooseDirectories = false;

            if (dlg.RunModal() == 1)
            {
                var url = dlg.Urls[0];
                if (url != null)
                {
                    OpenFile(url);
                }
            }
        }
       
    }
}
