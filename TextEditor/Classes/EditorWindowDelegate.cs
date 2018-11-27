using System;
using AppKit;
using System.IO;
using Foundation;

namespace TextEditor
{
    public class EditorWindowDelegate : NSWindowDelegate
    {
        public NSWindow Window { get; set;}
        public EditorWindowDelegate(NSWindow window)
        {
            this.Window = window;
        }
      
        public override bool WindowShouldClose(Foundation.NSObject sender){
            if (Window.DocumentEdited)
            {
                var alert = new NSAlert()
                {
                    AlertStyle = NSAlertStyle.Critical,
                    InformativeText = "Save changes to document before closing window?",
                    MessageText = "Save Document",
                };

                alert.AddButton("Save");
                alert.AddButton("Lose Changes");
                alert.AddButton("Cancel");
                var result = alert.RunSheetModal(Window);

                switch (result)
                {
                    case 1000:
                        //Saving
                        Console.WriteLine("Before defining the view controller");
                        var viewController = AppDelegate.FindViewController(Window.ContentViewController) as ViewController;
                        Console.WriteLine(viewController);
                        if (Window.RepresentedUrl != null) {
                            var path = Window.RepresentedUrl.Path;
                            File.WriteAllText(path, viewController.Text);
                            return true;
                        }
                        else {
                            Console.WriteLine("Stuff needs to be saved");
                            var dlg = new NSSavePanel
                            {
                                Title = "Save Document"
                            };
                            dlg.BeginSheet(Window, (rslt) =>
                            {
                                if (rslt == 1)
                                {
                                    var path = dlg.Url.Path;
                                    File.WriteAllText(path, viewController.Text);
                                    Window.DocumentEdited = false;
                                    viewController.View.Window.SetTitleWithRepresentedFilename(Path.GetFileName(path));
                                    viewController.View.Window.RepresentedUrl = dlg.Url;
                                    Window.Close();
                                }
                            });
                            return false;
                        }
                    case 1001:
                        //Lose changes
                        return true;

                    case 1002:
                        //Cancel
                        return false;
                }
            }
            return true;
        }
    }
}
