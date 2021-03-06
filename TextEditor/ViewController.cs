﻿using System;
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
        //NSTextStorage stuff
        public NSTextStorage TextStorage
        {
            get { return DocumentEditor.TextStorage; }
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
        }
        public override void ViewWillAppear()
        {
            base.ViewWillAppear();
            this.View.Window.Title = "untitled";
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