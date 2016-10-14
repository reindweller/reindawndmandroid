package md5579855c995bdb073d8182511e793544c;


public class DistressActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onCreateContextMenu:(Landroid/view/ContextMenu;Landroid/view/View;Landroid/view/ContextMenu$ContextMenuInfo;)V:GetOnCreateContextMenu_Landroid_view_ContextMenu_Landroid_view_View_Landroid_view_ContextMenu_ContextMenuInfo_Handler\n" +
			"n_onMenuItemSelected:(ILandroid/view/MenuItem;)Z:GetOnMenuItemSelected_ILandroid_view_MenuItem_Handler\n" +
			"";
		mono.android.Runtime.register ("App1.DistressActivity, App1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", DistressActivity.class, __md_methods);
	}


	public DistressActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == DistressActivity.class)
			mono.android.TypeManager.Activate ("App1.DistressActivity, App1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void onCreateContextMenu (android.view.ContextMenu p0, android.view.View p1, android.view.ContextMenu.ContextMenuInfo p2)
	{
		n_onCreateContextMenu (p0, p1, p2);
	}

	private native void n_onCreateContextMenu (android.view.ContextMenu p0, android.view.View p1, android.view.ContextMenu.ContextMenuInfo p2);


	public boolean onMenuItemSelected (int p0, android.view.MenuItem p1)
	{
		return n_onMenuItemSelected (p0, p1);
	}

	private native boolean n_onMenuItemSelected (int p0, android.view.MenuItem p1);

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
