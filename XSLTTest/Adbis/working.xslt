﻿<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="2.0">
  <xsl:output method="xml" indent="yes" />
  <!--PSMClass: "Purchase"-->
  <xsl:template match="/purchase">
    <purchase>
      <xsl:call-template name="TOP-Purchase-ELM">
        <xsl:with-param name="ci" select="./(* | @*)" />
      </xsl:call-template>
    </purchase>
  </xsl:template>
  <!--End of: PSMClass: "Purchase"-->
  <xsl:template name="TOP-Purchase-ELM">
    <xsl:param name="ci" as="item()*" />
    <xsl:apply-templates select="$ci[name() = 'purchase-date']" />
    <xsl:apply-templates select="$ci[name() = 'customer-info']" />
    <xsl:call-template name="TOP-Purchase-Items" />
  </xsl:template>
  <!--PSMClass: "CustomerInfo"-->
  <xsl:template match="/purchase/customer-info">
    <customer-info>
      <xsl:call-template name="TOP-Purchase-CustomerInfo-ELM">
        <xsl:with-param name="ci" select="./(* | @*)" />
      </xsl:call-template>
    </customer-info>
  </xsl:template>
  <!--End of: PSMClass: "CustomerInfo"-->
  <xsl:template name="TOP-Purchase-CustomerInfo-ELM">
    <xsl:param name="ci" as="item()*" />
    <xsl:apply-templates select="$ci[name() = 'customer']" />
  </xsl:template>
  <!--PSMClass: "Items"-->
  <xsl:template name="TOP-Purchase-Items">
    <items>
      <xsl:call-template name="TOP-Purchase-Items-ELM" />
    </items>
  </xsl:template>
  <!--End of: PSMClass: "Items"-->
  <xsl:template name="TOP-Purchase-Items-ELM">
    <xsl:apply-templates select="item" />
  </xsl:template>
  <!--PSMClass: "Customer"-->
  <xsl:template match="/purchase/customer-info/customer">
    <customer>
      <xsl:call-template name="TOP-Purchase-CustomerInfo-Customer-ELM">
        <xsl:with-param name="ci" select="./(* | @*)" />
      </xsl:call-template>
    </customer>
  </xsl:template>
  <!--End of: PSMClass: "Customer"-->
  <xsl:template name="TOP-Purchase-CustomerInfo-Customer-ELM">
    <xsl:param name="ci" as="item()*" />
    <xsl:apply-templates select="$ci[name() = 'customer-no']" />
    <xsl:apply-templates select="../address" />
    <xsl:call-template name="TOP-Purchase-CustomerInfo-Customer-CustEmail" />
  </xsl:template>
  <!--PSMClass: "Address"-->
  <xsl:template match="/purchase/customer-info/address">
    <delivery-address>
      <xsl:call-template name="TOP-Purchase-CustomerInfo-Customer-Address-ELM">
        <xsl:with-param name="ci" select="./(* | @*)" />
      </xsl:call-template>
    </delivery-address>
  </xsl:template>
  <!--End of: PSMClass: "Address"-->
  <xsl:template name="TOP-Purchase-CustomerInfo-Customer-Address-ELM">
    <xsl:param name="ci" as="item()*" />
    <xsl:apply-templates select="$ci[name() = 'zip']" />
    <xsl:apply-templates select="$ci[name() = 'city']" />
    <xsl:apply-templates select="$ci[name() = 'street']" />
  </xsl:template>
  <!--PSMClass: "CustEmail"-->
  <xsl:template name="TOP-Purchase-CustomerInfo-Customer-CustEmail">
    <emails>
      <xsl:call-template name="TOP-Purchase-CustomerInfo-Customer-CustEmail-ELM" />
    </emails>
  </xsl:template>
  <!--End of: PSMClass: "CustEmail"-->
  <xsl:template name="TOP-Purchase-CustomerInfo-Customer-CustEmail-ELM">
    <xsl:apply-templates select="email" />
  </xsl:template>
  <!--PSMClass: "Product"-->
  <xsl:template match="/purchase/item/product">
    <product>
      <xsl:call-template name="TOP-Purchase-Items-Item-Product-ELM">
        <xsl:with-param name="ci" select="./(* | @*)" />
      </xsl:call-template>
    </product>
  </xsl:template>
  <!--End of: PSMClass: "Product"-->
  <xsl:template name="TOP-Purchase-Items-Item-Product-ELM">
    <xsl:param name="ci" as="item()*" />
    <xsl:apply-templates select="$ci[name() = 'code']" />
    <xsl:apply-templates select="$ci[name() = 'subcode']" />
    <xsl:apply-templates select="$ci[name() = 'title']" />
  </xsl:template>
  <!--Blue nodes template-->
  <xsl:template match="item" priority="0">
    <xsl:copy>
      <xsl:copy-of select="@*" />
      <xsl:apply-templates select="*" />
    </xsl:copy>
  </xsl:template>
  <!--Green nodes template-->
  <xsl:template match="qty | purchase-date | customer-no | zip | city | street | code | subcode | title | amount | unit-price | email" priority="0">
    <xsl:copy-of select="." />
  </xsl:template>
</xsl:stylesheet>