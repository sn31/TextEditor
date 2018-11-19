using System;
using AppKit;
using Foundation;

namespace TextEditor
{
    public partial class ViewController : NSViewController
    {
        #region Computed Properties
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

        public bool DocumentEdited
        {
            get { return View.Window.DocumentEdited; }
            set { View.Window.DocumentEdited = value; }
        }

        public string Text
        {
            get { return DocumentEditor.Value; }
            set { DocumentEditor.Value = value; }
        }

        #endregion
        #region Constructors
        public ViewController(IntPtr handle) : base(handle)
        {
        }
        #endregion

        public string FilePath { get; set; } = "";

        #region Override Methods
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SourceList.Initialize();

            var library = new SourceListItem("Library");
            library.AddItem("Venues", "house.png", () => {
                Console.WriteLine("Venue Selected");
            });
            library.AddItem("Singers", "group.png");
            library.AddItem("Genre", "cards.png");
            library.AddItem("Publishers", "box.png");
            library.AddItem("Artist", "person.png");
            library.AddItem("Music", "album.png");
            SourceList.AddItem(library);

            // Add Rotation 
            var rotation = new SourceListItem("Rotation");
            rotation.AddItem("View Rotation", "redo.png");
            SourceList.AddItem(rotation);

            // Add Kiosks
            var kiosks = new SourceListItem("Kiosks");
            kiosks.AddItem("Sign-in Station 1", "imac");
            kiosks.AddItem("Sign-in Station 2", "ipad");
            SourceList.AddItem(kiosks);

            // Display side list
            SourceList.ReloadData();
            SourceList.ExpandItem(null, true);
            // Do any additional setup after loading the view.

        }

        public override void ViewWillAppear()
        {
            base.ViewWillAppear();

            // Set Window Title
            this.View.Window.Title = "untitled";

            View.Window.WillClose += (sender, e) => {
                // is the window dirty?
                if (DocumentEdited)
                {
                    var alert = new NSAlert()
                    {
                        AlertStyle = NSAlertStyle.Critical,
                        InformativeText = "We need to give the user the ability to save the document here...",
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
        #endregion

    }
}