using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;

namespace TextEditor
{
    public partial class ExplorerViewController : NSViewController
    {
        #region Constructors

        // Called when created from unmanaged code
        public ExplorerViewController(IntPtr handle) : base(handle)
        {
         
        }
     
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SourceList.Initialize();

            var draft = new SourceListItem("Draft");
            draft.AddItem("Draft 1", "house.png", () => {
                Console.WriteLine("Draft selected");
            });
            draft.AddItem("Draft 2", "group.png");
            draft.AddItem("Draft 3", "cards.png");
           
            SourceList.AddItem(draft);

            // Add research 
            var research = new SourceListItem("Research");
            research.AddItem("Book 1", "redo.png");
            research.AddItem("Book 2", "redo.png");
            SourceList.AddItem(research);

            // Add trash
            var trash = new SourceListItem("Trash");
            trash.AddItem("Trash item 1", "imac");
            trash.AddItem("Trash item 2", "ipad");
            SourceList.AddItem(trash);

            // Display side list
            SourceList.ReloadData();
            SourceList.ExpandItem(null, true);

        }
        #endregion
    }
}
