<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="2.0">
  <!-- Template generated by eXolutio on 27.7.2011 21:16 
    from D:\Programování\EvoXSVN\XSLTTest\Renames\renames.eXo. -->
  <xsl:output method="xml" indent="yes" />
  <!--PSMClass: "Purchase"-->
  <xsl:template match="/purchase">
    <purchaseRQ>
      <xsl:call-template name="TOP-Purchase-ATT">
        <xsl:with-param name="ci" select="./(* | @*)" />
      </xsl:call-template>
      <xsl:call-template name="TOP-Purchase-ELM">
        <xsl:with-param name="ci" select="./(* | @*)" />
      </xsl:call-template>
    </purchaseRQ>
  </xsl:template>
  <!--End of: PSMClass: "Purchase"-->
  <xsl:template name="TOP-Purchase-ATT">
    <xsl:param name="ci" as="item()*" />
    <xsl:apply-templates select="$ci[name() = 'purchase-date']" />
  </xsl:template>
  <xsl:template name="TOP-Purchase-ELM">
    <xsl:param name="ci" as="item()*" />
    <xsl:apply-templates select="$ci[name() = 'customer-info']" />
    <xsl:apply-templates select="$ci[name() = 'item']" />
  </xsl:template>
  <!--PSMAttribute: "Purchase.date" 1..1-->
  <xsl:template match="/purchase/@purchase-date">
    <xsl:attribute name="date">
      <xsl:value-of select="." />
    </xsl:attribute>
  </xsl:template>
  <!--End of: PSMAttribute: "Purchase.date" 1..1-->
  <!--PSMClass: "Item"-->
  <xsl:template match="/purchase/item">
    <pur-item>
      <xsl:call-template name="TOP-Purchase-Item-ELM">
        <xsl:with-param name="ci" select="./(* | @*)" />
      </xsl:call-template>
    </pur-item>
  </xsl:template>
  <!--End of: PSMClass: "Item"-->
  <xsl:template name="TOP-Purchase-Item-ELM">
    <xsl:param name="ci" as="item()*" />
    <xsl:apply-templates select="$ci[name() = 'product']" />
    <!-- expanded group -->
    <xsl:apply-templates select="$ci[name() = 'amount'] | $ci[name() = 'unit-price']" />
  </xsl:template>
  <!--PSMAttribute: "ItemI.price" 1..1-->
  <xsl:template match="/purchase/item/unit-price">
    <price>
      <xsl:value-of select="." />
    </price>
  </xsl:template>
  <!--End of: PSMAttribute: "ItemI.price" 1..1-->
  <!--PSMAttribute: "ArbitraryGroupNode.groupatt-N" 1..1-->
  <xsl:template match="/purchase/item/product/groupatt">
    <groupatt-N>
      <xsl:value-of select="." />
    </groupatt-N>
  </xsl:template>
  <!--End of: PSMAttribute: "ArbitraryGroupNode.groupatt-N" 1..1-->
  <!--PSMClass: "class1"-->
  <xsl:template match="/purchase/item/product/child1">
    <child1-N>
      <xsl:call-template name="TOP-Purchase-Item-Product-ArbitraryGroupNode-class1-ATT">
        <xsl:with-param name="ci" select="./(* | @*)" />
      </xsl:call-template>
    </child1-N>
  </xsl:template>
  <!--End of: PSMClass: "class1"-->
  <xsl:template name="TOP-Purchase-Item-Product-ArbitraryGroupNode-class1-ATT">
    <xsl:param name="ci" as="item()*" />
    <xsl:apply-templates select="$ci[name() = 'a1']" />
  </xsl:template>
  <!--PSMClass: "class2"-->
  <xsl:template match="/purchase/item/product/child2">
    <child2-N>
      <xsl:call-template name="TOP-Purchase-Item-Product-ArbitraryGroupNode-class2-ATT">
        <xsl:with-param name="ci" select="./(* | @*)" />
      </xsl:call-template>
      <xsl:call-template name="TOP-Purchase-Item-Product-ArbitraryGroupNode-class2-ELM">
        <xsl:with-param name="ci" select="./(* | @*)" />
      </xsl:call-template>
    </child2-N>
  </xsl:template>
  <!--End of: PSMClass: "class2"-->
  <xsl:template name="TOP-Purchase-Item-Product-ArbitraryGroupNode-class2-ATT">
    <xsl:param name="ci" as="item()*" />
    <xsl:apply-templates select="$ci[name() = 'nodeAttA']" />
  </xsl:template>
  <xsl:template name="TOP-Purchase-Item-Product-ArbitraryGroupNode-class2-ELM">
    <xsl:param name="ci" as="item()*" />
    <xsl:apply-templates select="$ci[name() = 'nodeAttE']" />
  </xsl:template>
  <!--PSMAttribute: "class1.a1ren" 1..1-->
  <xsl:template match="/purchase/item/product/child1/@a1">
    <xsl:attribute name="a1ren">
      <xsl:value-of select="." />
    </xsl:attribute>
  </xsl:template>
  <!--End of: PSMAttribute: "class1.a1ren" 1..1-->
  <!--PSMAttribute: "class2.nodeAttEren" 1..1-->
  <xsl:template match="/purchase/item/product/child2/nodeAttE">
    <nodeAttEren>
      <xsl:value-of select="." />
    </nodeAttEren>
  </xsl:template>
  <!--End of: PSMAttribute: "class2.nodeAttEren" 1..1-->
  <!--PSMAttribute: "class2.nodeAttAren" 1..1-->
  <xsl:template match="/purchase/item/product/child2/@nodeAttA">
    <xsl:attribute name="nodeAttAren">
      <xsl:value-of select="." />
    </xsl:attribute>
  </xsl:template>
  <!--End of: PSMAttribute: "class2.nodeAttAren" 1..1-->
  <!--Blue nodes template-->
  <xsl:template match="product" priority="0">
    <xsl:copy>
      <xsl:copy-of select="@*" />
      <xsl:apply-templates select="*" />
    </xsl:copy>
  </xsl:template>
  <!--Green nodes template-->
  <xsl:template match="customer | address | customer-info | customer-no | email | zip | city | street | code | subcode | title | weight | amount" priority="0">
    <xsl:copy-of select="." />
  </xsl:template>
</xsl:stylesheet>