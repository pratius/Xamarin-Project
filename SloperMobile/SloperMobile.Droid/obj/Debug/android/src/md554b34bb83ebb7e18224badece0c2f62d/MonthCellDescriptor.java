package md554b34bb83ebb7e18224badece0c2f62d;


public class MonthCellDescriptor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_toString:()Ljava/lang/String;:GetToStringHandler\n" +
			"";
		mono.android.Runtime.register ("XLabs.Forms.Controls.MonoDroid.TimesSquare.MonthCellDescriptor, XLabs.Forms.Droid, Version=2.0.5610.35391, Culture=neutral, PublicKeyToken=null", MonthCellDescriptor.class, __md_methods);
	}


	public MonthCellDescriptor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == MonthCellDescriptor.class)
			mono.android.TypeManager.Activate ("XLabs.Forms.Controls.MonoDroid.TimesSquare.MonthCellDescriptor, XLabs.Forms.Droid, Version=2.0.5610.35391, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public java.lang.String toString ()
	{
		return n_toString ();
	}

	private native java.lang.String n_toString ();

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
