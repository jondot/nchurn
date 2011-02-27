<!DOCTYPE internal [
    <!ENTITY nbsp " ">
]>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt">

<xsl:template match="/NChurnAnalysisResult">
<html>
  <head>
    <title>Churn Report</title>
    <style>
      BODY { font-family: Trebuchet MS; font-size: 10pt; }
      TD { font-family: Trebuchet MS; font-size: 10pt; }
      .title { font-size: 20pt; font-weight: bold; }
      div.c { width:200px }
      div.c > div {
        background-color: #ACE97C; 
        height: 12px;
      }
                  
    </style>
  </head>
  <body>
    <div class="title">Churn Report</div>
		<table>
		<xsl:apply-templates select="FileChurns/FileChurn">
			<xsl:with-param name="maxchurn" select="(FileChurns/FileChurn[1])/Value"/>
		</xsl:apply-templates>
		</table>
  </body>
</html>
</xsl:template>
    
<xsl:template match="FileChurn">
  <xsl:param name="maxchurn"/>
    <tr>
      <td class="file-churn"><xsl:value-of select="File"/></td>
      <td><xsl:value-of select="Value"/></td>
      <td>
        <div class="c"><div style="width:{floor(Value*100 div $maxchurn)}%"></div></div>
      </td>
    </tr>
  </xsl:template>   
</xsl:stylesheet>

