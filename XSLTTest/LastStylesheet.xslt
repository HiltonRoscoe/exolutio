<?xml version="1.0" encoding="utf-16"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  version="3.0" xmlns:xs="http://www.w3.org/2001/XMLSchema" 
  xmlns:const="http://eXolutio.com/oclX/types/constructors" 
  xmlns:oclX="http://eXolutio.com/oclX/functional" 
  xmlns:oclXin="http://eXolutio.com/oclX/functional/internal" 
  xmlns:oclDate="http://eXolutio.com/oclX/types/date" 
  xmlns:oclString="http://eXolutio.com/oclX/types/string">
  <xsl:import href="file:/D:\Programování\EvoXSVN\OclX\oclX-functional.xsl" />
  <xsl:output method="xml" indent="yes" />
  <!--PSMClass: "C1"-->
  <xsl:template match="/C1">
    <C1>
      <xsl:call-template name="TOP-C1-ELM">
        <xsl:with-param name="ci" select="./(* | @*)" />
      </xsl:call-template>
    </C1>
  </xsl:template>
  <!--End of: PSMClass: "C1"-->
  <xsl:template name="TOP-C1-ELM">
    <xsl:param name="ci" as="item()*" />
    <!--ref PSMAttribute: "C1.a1" 1..1-->
    <xsl:apply-templates select="$ci[name() = 'a1']" />
    <!--ref PSMClass: "CLeft"-->
    <xsl:variable name="CLeft-new-values" select="oclX:collect(oclX:select($ci[name() = 'C2'], function($c2p) { $c2p/a2 gt 0 }), function($c2pc) { const:TOP-C1-CLeft-CONST('CLeft', data($c2pc/a3)) })" as="item()*" />
    <xsl:sequence select="$CLeft-new-values" />
    <!--ref PSMClass: "CRight"-->
    <xsl:call-template name="TOP-C1-CRight">
      <xsl:with-param name="ci" select="$ci" />
    </xsl:call-template>
  </xsl:template>
  <!--PSMAttribute: "C1.a1" 1..1-->
  <xsl:template match="/C1/a1">
    <a1>
      <xsl:value-of select="." />
    </a1>
  </xsl:template>
  <!--End of: PSMAttribute: "C1.a1" 1..1-->
  <xsl:function name="const:TOP-C1-CLeft-CONST" as="item()*">
    <xsl:param name="element-name" as="xs:string" />
    <xsl:param name="a4" as="item()*" />
    <xsl:choose>
      <xsl:when test="$element-name ne ''">
        <xsl:element name="{$element-name}">
          <xsl:sequence select="$a4" />
        </xsl:element>
      </xsl:when>
      <xsl:otherwise>
        <xsl:sequence select="$a4" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:function>
  <!--PSMClass: "CLeft"-->
  <xsl:template name="TOP-C1-CLeft">
    <xsl:param name="ci" as="item()*" />
    <CLeft>
      <xsl:call-template name="TOP-C1-CLeft-ELM">
        <xsl:with-param name="ci" select="./(* | @*)" />
      </xsl:call-template>
    </CLeft>
  </xsl:template>
  <!--End of: PSMClass: "CLeft"-->
  <xsl:template name="TOP-C1-CLeft-ELM">
    <xsl:param name="ci" as="item()*" />
    <!--ref PSMAttribute: "CLeft.a4" 1..1-->
    <xsl:call-template name="TOP-C1-CLeft-a4-IG" />
  </xsl:template>
  <!--PSMClass: "CRight"-->
  <xsl:template name="TOP-C1-CRight">
    <xsl:param name="ci" as="item()*" />
    <CRight>
      <xsl:call-template name="TOP-C1-CRight-ELM">
        <xsl:with-param name="ci" select="./(* | @*)" />
      </xsl:call-template>
    </CRight>
  </xsl:template>
  <!--End of: PSMClass: "CRight"-->
  <xsl:template name="TOP-C1-CRight-ELM">
    <xsl:param name="ci" as="item()*" />
    <!--ref PSMAttribute: "CRight.a5" 1..1-->
    <xsl:call-template name="TOP-C1-CRight-a5-IG" />
  </xsl:template>
  <!-- Instance generators -->
  <xsl:template name="TOP-C1-CLeft-a4-IG">
    <xsl:param name="count" as="item()" select="1" />
    <xsl:for-each select="1 to $count">
      <a4>a4<xsl:value-of select="current()" /></a4>
    </xsl:for-each>
  </xsl:template>
  <xsl:template name="TOP-C1-CRight-a5-IG">
    <xsl:param name="count" as="item()" select="1" />
    <xsl:for-each select="1 to $count">
      <a5>a5<xsl:value-of select="current()" /></a5>
    </xsl:for-each>
  </xsl:template>
  <!--No blue nodes-->
  <!--No blue nodes-->
</xsl:stylesheet>