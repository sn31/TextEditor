using System;

using AppKit;
using Foundation;

namespace TextEditor
{
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Do any additional setup after loading the view.
        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }
        public bool DocumentEdited {
            get { return View.Window.DocumentEdited; }
            set { View.Window.DocumentEdited = value; }
        }
        public string Text
        {
            get { return DocumentEditor.Value; }
            set { DocumentEditor.Value = value; }
        }
        public override void ViewWillAppear()
        {
            base.ViewWillAppear();

            this.View.Window.Title = "untitled";

            View.Window.WillClose += (sender, e) =>
            {
                if (DocumentEdited)
                {
                    var alert = new NSAlert()
                    {
                        AlertStyle = NSAlertStyle.Critical,
                        InformativeText = "Save document before closing?",
                        MessageText = "Save Document",
                    };
                    alert.RunModal();
                }
            };
        }
        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            // Show when the document is edited
            DocumentEditor.TextDidChange += (sender, e) => {
                // Mark the document as dirty
                DocumentEdited = true;
            };

            // Overriding this delegate is required to monitor the TextDidChange event
            DocumentEditor.ShouldChangeTextInRanges += (NSTextView view, NSValue[] values, string[] replacements) => {
                return true;
            };

        }
    }
}
