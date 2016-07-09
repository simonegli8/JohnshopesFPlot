<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="default.aspx.cs" Inherits="_Default" %>
<%@ Register Assembly="JohnsHope.FPlot.Library" Namespace="JohnsHope.FPlot.Library.Web"
	TagPrefix="fplot" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
	<title>Johns Hope's FPlot Web Demo</title>
</head>
<body>
	<form id="form" runat="server">
		<h1>A sine/cosine curve</h1>
		<asp:DropDownList id="list" runat="server" AutoPostBack="true" OnSelectedIndexChanged="FunctionChanged">
			<asp:ListItem>Sine</asp:ListItem>
			<asp:ListItem>Cosine</asp:ListItem>
		</asp:DropDownList><br/>
		<fplot:PlotImage id="plot" runat="server" Height="354px" Width="493px" TempPath="~/temp" EnableZooming="true" />
		
		<h1>The Mandelbrot Set</h1>
		<fplot:PlotImage id="plot2" runat="server" Height="354px" Width="493px" TempPath="~/temp" EnableZooming="true" />
		<p>
			You can zoom in or out by dragging over the images with the left mouse button pressed.
			If you press the shift key during dragging, you will zoom out.
		</p>
	</form>
</body>
</html>
