<%@ Control ClassName="CacheViewer" Language="C#" %>
<%@ Import Namespace="System.Data" %>

<script runat="server">
  public void Refresh()
  {
	DataTable dt = GetCacheContents();
	grid.DataSource = dt;
	grid.DataBind();
  }


  private DataTable GetCacheContents()
  {
	DataTable dt = CreateDataTable();
	
	foreach(DictionaryEntry elem in Cache)
	{
		AddItemToTable(dt, elem);	
	}

	return dt;
  }

  private DataTable CreateDataTable()
  {
	DataTable dt = new DataTable();
 	dt.Columns.Add("Key", typeof(string));
 	dt.Columns.Add("Value", typeof(string));
	return dt;
  }

  public void Clear()
  {
	foreach(DictionaryEntry elem in Cache)
	{
		String s = elem.Key.ToString();
		Cache.Remove(s);
	}
  }

  private void AddItemToTable(DataTable dt, DictionaryEntry elem)
  {
	DataRow row = dt.NewRow();
	row["Key"] = elem.Key.ToString();
	row["Value"] = elem.Value.ToString();
	dt.Rows.Add(row);
  }
</script>


<asp:datagrid runat="server" id="grid" width="100%" 
	CellPadding="2" GridLines="None"
	Font-name="verdana" Font-size="8pt">
	<AlternatingItemStyle BackColor="PaleGoldenrod" />
	<HeaderStyle Font-Bold="True" Font-size="130%" BackColor="Tan" /> 
</asp:datagrid>
