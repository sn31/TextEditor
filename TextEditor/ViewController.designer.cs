// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace TextEditor
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSTextView DocumentEditor { get; set; }

		[Outlet]
		Foundation.NSObject SourceList { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (DocumentEditor != null) {
				DocumentEditor.Dispose ();
				DocumentEditor = null;
			}

			if (SourceList != null) {
				SourceList.Dispose ();
				SourceList = null;
			}
		}
	}
}
