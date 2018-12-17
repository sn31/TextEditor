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


        public override NSApplicationTerminateReply ApplicationShouldTerminate(NSApplication sender)
        {
            foreach (NSWindow window in NSApplication.SharedApplication.Windows){
                if (window.Delegate != null && !window.Delegate.WindowShouldClose(this)){
                    return NSApplicationTerminateReply.Cancel;
                }
            }
            Console.WriteLine("Application should terminate");
            return NSApplicationTerminateReply.Now;
        }
       
        [Export("newDocument:")]
        void NewDocument(NSObject sender)
        {
            // Get new window
            var storyboard = NSStoryboard.FromName("Main", null);
            var controller = storyboard.InstantiateControllerWithIdentifier("MainWindow") as NSWindowController;

            // Display
            controller.ShowWindow(this);

            // Set the title
            controller.Window.Title = (++UntitledWindowCount == 1) ? "untitled" : string.Format("untitled {0}", UntitledWindowCount);

        }

        public static ViewController FindViewController(NSViewController root)
        {
            foreach (var controller in root.ChildViewControllers)
            {
                if (controller is ViewController)
                {
                    return controller as ViewController;
                }

                if (controller.ChildViewControllers.Length > 0) {
                    var result = FindViewController(controller);
                    if (result != null) {
                        return result;
                    }
                }
            }

            return null;
        }

        private bool OpenFile(NSUrl url)
        {
            Console.WriteLine("OpenFile is ran");
            var good = false;

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
                var viewController = FindViewController(controller.ContentViewController);

                // Display
                controller.ShowWindow(this);
                NSDictionary<NSString,NSObject> newDict = new NSDictionary<NSString, NSObject>();
                NSError errors = new NSError();
                NSAttributedStringDocumentAttributes attributes = new NSAttributedStringDocumentAttributes();
                attributes.DocumentType = NSDocumentType.RTF;
                viewController.TextStorage.ReadFromUrl(url, attributes, ref newDict, ref errors); 
                viewController.View.Window.SetTitleWithRepresentedFilename(Path.GetFileName(path));
                viewController.View.Window.RepresentedUrl = url;

                // Add document to the Open Recent menu
                NSDocumentController.SharedDocumentController.NoteNewRecentDocumentURL(url);

                // Make as successful
                good = true;
            
            // Return results
            return good;
        }

        [Export("openDocument:")]
        void OpenDialog(NSObject sender)
        {
            Console.WriteLine("OpenDialog is ran");
            var dlg = NSOpenPanel.OpenPanel;
            dlg.CanChooseFiles = true;
            dlg.CanChooseDirectories = true;

            if (dlg.RunModal() == 1)
            {
                var url = dlg.Urls[0];
                if (url != null)
                {
                    OpenFile(url);
                }
            }
        }
        public override bool OpenFile(NSApplication sender, string filename)
        {
            // Trap all errors
            try
            {
                filename = filename.Replace(" ", "%20");
                var url = new NSUrl("file://" + filename);
                return OpenFile(url);
            }
            catch
            {
                return false;
            }
        }


    }
}
