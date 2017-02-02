package mono.com.syncfusion.gauges.SfCircularGauge;


public class CircularPointer_PointerPositionChangedListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.syncfusion.gauges.SfCircularGauge.CircularPointer.PointerPositionChangedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onPointerPositionChanged:(Ljava/lang/Object;Lcom/syncfusion/gauges/SfCircularGauge/PointerPosition;)V:GetOnPointerPositionChanged_Ljava_lang_Object_Lcom_syncfusion_gauges_SfCircularGauge_PointerPosition_Handler:Com.Syncfusion.Gauges.SfCircularGauge.CircularPointer/IPointerPositionChangedListenerInvoker, Syncfusion.SfGauge.Android\n" +
			"";
		mono.android.Runtime.register ("Com.Syncfusion.Gauges.SfCircularGauge.CircularPointer+IPointerPositionChangedListenerImplementor, Syncfusion.SfGauge.Android, Version=14.4451.0.15, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89", CircularPointer_PointerPositionChangedListenerImplementor.class, __md_methods);
	}


	public CircularPointer_PointerPositionChangedListenerImplementor () throws java.lang.Throwable
	{
		super ();
		if (getClass () == CircularPointer_PointerPositionChangedListenerImplementor.class)
			mono.android.TypeManager.Activate ("Com.Syncfusion.Gauges.SfCircularGauge.CircularPointer+IPointerPositionChangedListenerImplementor, Syncfusion.SfGauge.Android, Version=14.4451.0.15, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89", "", this, new java.lang.Object[] {  });
	}


	public void onPointerPositionChanged (java.lang.Object p0, com.syncfusion.gauges.SfCircularGauge.PointerPosition p1)
	{
		n_onPointerPositionChanged (p0, p1);
	}

	private native void n_onPointerPositionChanged (java.lang.Object p0, com.syncfusion.gauges.SfCircularGauge.PointerPosition p1);

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
