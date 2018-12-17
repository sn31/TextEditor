using System;
using AppKit;
using System.IO;
using Foundation;
using TextEditor.Classes;

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
                        return Utilities.SaveDocument(Window);

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
