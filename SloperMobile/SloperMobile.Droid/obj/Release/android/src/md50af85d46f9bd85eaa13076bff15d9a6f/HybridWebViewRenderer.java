package md50af85d46f9bd85eaa13076bff15d9a6f;


public class HybridWebViewRenderer
	extends md5b60ffeb829f638581ab2bb9b1a7f4f3f.ViewRenderer_2
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("XLabs.Forms.Controls.HybridWebViewRenderer, XLabs.Forms.Droid, Version=2.0.5610.35391, Culture=neutral, PublicKeyToken=null", HybridWebViewRenderer.class, __md_methods);
	}


	public HybridWebViewRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2) throws java.lang.Throwable
	{
		super (p0, p1, p2);
		if (getClass () == HybridWebViewRenderer.class)
			mono.android.TypeManager.Activate ("XLabs.Forms.Controls.HybridWebViewRenderer, XLabs.Forms.Droid, Version=2.0.5610.35391, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public HybridWebViewRenderer (android.content.Context p0, android.util.AttributeSet p1) throws java.lang.Throwable
	{
		super (p0, p1);
		if (getClass () == HybridWebViewRenderer.class)
			mono.android.TypeManager.Activate ("XLabs.Forms.Controls.HybridWebViewRenderer, XLabs.Forms.Droid, Version=2.0.5610.35391, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}


	public HybridWebViewRenderer (android.content.Context p0) throws java.lang.Throwable
	{
		super (p0);
		if (getClass () == HybridWebViewRenderer.class)
			mono.android.TypeManager.Activate ("XLabs.Forms.Controls.HybridWebViewRenderer, XLabs.Forms.Droid, Version=2.0.5610.35391, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}

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
