﻿<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="2.0">
  <xsl:output method="xml" indent="yes" />
  <!--PSMClass: "Purchase"-->
  <xsl:template match="/purchase-request">
    <purchase-request>
      <xsl:call-template name="TOP-Purchase-ATT">
        <xsl:with-param name="ci" select="./(* | @*)" />
      </xsl:call-template>
      <xsl:call-template name="TOP-Purchase-ELM">
        <xsl:with-param name="ci" select="./(* | @*)" />
      </xsl:call-template>
    </purchase-request>
  </xsl:template>
  <!--End of: PSMClass: "Purchase"-->
  <xsl:template name="TOP-Purchase-ATT">
    <xsl:param name="ci" as="item()*" />
    <!--ref PSMAttribute: "Purchase.purchase-date" 1..1-->
    <xsl:apply-templates select="$ci[name() = 'purchase-date']" />
  </xsl:template>
  <xsl:template name="TOP-Purchase-ELM">
    <xsl:param name="ci" as="item()*" />
    <!--ref PSMClass: "CustomerInfo"-->
    <xsl:apply-templates select="$ci[name() = 'customer-info']" />
    <!--ref PSMClass: "SalesAssistant"-->
    <xsl:call-template name="TOP-Purchase-SalesAssistant-ELM">
      <xsl:with-param name="ci" select="$ci[name() = 'emp-no']" />
    </xsl:call-template>
    <!--ref PSMClass: "Items"-->
    <xsl:call-template name="TOP-Purchase-Items" />
    <!--ref PSMClass: "defaultname"-->
    <xsl:call-template name="TOP-Purchase-defaultname" />
  </xsl:template>
  <!--PSMClass: "CustomerInfo"-->
  <xsl:template match="/purchase-request/customer-info">
    <customer-info>
      <xsl:call-template name="TOP-Purchase-CustomerInfo-ELM">
        <xsl:with-param name="ci" select="./(* | @*)" />
      </xsl:call-template>
    </customer-info>
  </xsl:template>
  <!--End of: PSMClass: "CustomerInfo"-->
  <xsl:template name="TOP-Purchase-CustomerInfo-ELM">
    <xsl:param name="ci" as="item()*" />
    <!--ref PSMClass: "Customer"-->
    <xsl:apply-templates select="$ci[name() = 'customer']" />
  </xsl:template>
  <xsl:template name="TOP-Purchase-SalesAssistant-ELM">
    <xsl:param name="ci" as="item()*" />
    <!--ref PSMAttribute: "SalesAssistant.emp-no" 1..1-->
    <xsl:apply-templates select="$ci[name() = 'emp-no']" />
    <xsl:call-template name="TOP-Purchase-SalesAssistant-emp-no-IG">
      <xsl:with-param name="count" select="1 - count($ci[name() = 'emp-no'])" />
    </xsl:call-template>
    <!--ref PSMAttribute: "SalesAssistant.name" 1..1-->
    <xsl:call-template name="TOP-Purchase-SalesAssistant-name-IG" />
  </xsl:template>
  <!--PSMClass: "Items"-->
  <xsl:template name="TOP-Purchase-Items">
    <items>
      <xsl:call-template name="TOP-Purchase-Items-ELM" />
    </items>
  </xsl:template>
  <!--End of: PSMClass: "Items"-->
  <xsl:template name="TOP-Purchase-Items-ELM">
    <!--ref PSMClass: "Item"-->
    <xsl:for-each-group select="product | item-info" group-starting-with="product">
      <xsl:if test="position() le 5">
        <xsl:variable name="ci" select="current-group()" />
        <xsl:apply-templates select="$ci[name() = 'product']" />
        <xsl:apply-templates select="$ci[name() = 'item-info']" />
      </xsl:if>
    </xsl:for-each-group>
    <xsl:call-template name="TOP-Purchase-Items-Item-ELM-IG">
      <xsl:with-param name="count" select="4 - count(product)" />
    </xsl:call-template>
  </xsl:template>
  <!--PSMClass: "defaultname"-->
  <xsl:template name="TOP-Purchase-defaultname">
    <AAA />
  </xsl:template>
  <!--End of: PSMClass: "defaultname"-->
  <!--PSMClass: "Customer"-->
  <xsl:template match="/purchase-request/customer-info/customer">
    <customer>
      <xsl:call-template name="TOP-Purchase-CustomerInfo-Customer-ATT">
        <xsl:with-param name="ci" select="./(* | @*)" />
      </xsl:call-template>
      <xsl:call-template name="TOP-Purchase-CustomerInfo-Customer-ELM">
        <xsl:with-param name="ci" select="./(* | @*)" />
      </xsl:call-template>
    </customer>
  </xsl:template>
  <!--End of: PSMClass: "Customer"-->
  <xsl:template name="TOP-Purchase-CustomerInfo-Customer-ATT">
    <xsl:param name="ci" as="item()*" />
    <!--ref PSMAttribute: "Customer.customer-no" 1..1-->
    <xsl:apply-templates select="$ci[name() = 'customer-no']" />
  </xsl:template>
  <xsl:template name="TOP-Purchase-CustomerInfo-Customer-ELM">
    <xsl:param name="ci" as="item()*" />
    <!--ref PSMClass: "Address"-->
    <xsl:for-each-group select="../postcode | ../city | ../street" group-starting-with="postcode">
      <xsl:call-template name="TOP-Purchase-CustomerInfo-Customer-Address">
        <xsl:with-param name="ci" select="current-group()" />
      </xsl:call-template>
    </xsl:for-each-group>
    <xsl:call-template name="TOP-Purchase-CustomerInfo-Customer-Address-ELM-IG">
      <xsl:with-param name="count" select="1 - count(../postcode)" />
    </xsl:call-template>
    <!--ref PSMClass: "CustEmail"-->
    <xsl:call-template name="TOP-Purchase-CustomerInfo-Customer-CustEmail-ELM">
      <xsl:with-param name="ci" select="$ci[name() = 'email']" />
    </xsl:call-template>
  </xsl:template>
  <!--PSMClass: "Address"-->
  <xsl:template name="TOP-Purchase-CustomerInfo-Customer-Address">
    <xsl:param name="ci" as="item()*" />
    <delivery-address>
      <xsl:call-template name="TOP-Purchase-CustomerInfo-Customer-Address-ELM">
        <xsl:with-param name="ci" select="$ci" />
      </xsl:call-template>
    </delivery-address>
  </xsl:template>
  <!--End of: PSMClass: "Address"-->
  <xsl:template name="TOP-Purchase-CustomerInfo-Customer-Address-ELM">
    <xsl:param name="ci" as="item()*" />
    <!--ref PSMAttribute: "Address.postcode" 1..1-->
    <xsl:apply-templates select="$ci[name() = 'postcode']" />
    <!--ref PSMAttribute: "Address.city" 1..1-->
    <xsl:apply-templates select="$ci[name() = 'city']" />
    <!--ref PSMAttribute: "Address.street" 1..1-->
    <xsl:apply-templates select="$ci[name() = 'street']" />
  </xsl:template>
  <xsl:template name="TOP-Purchase-CustomerInfo-Customer-CustEmail-ELM">
    <xsl:param name="ci" as="item()*" />
    <!--ref PSMAttribute: "CustEmail.email" 3..5-->
    <xsl:apply-templates select="$ci[name() = 'email'][position() le 5]" />
    <xsl:call-template name="TOP-Purchase-CustomerInfo-Customer-CustEmail-email-IG">
      <xsl:with-param name="count" select="3 - count(email)" />
    </xsl:call-template>
  </xsl:template>
  <!--PSMClass: "Product"-->
  <xsl:template match="/purchase-request/product">
    <product>
      <xsl:call-template name="TOP-Purchase-Items-Item-Product-ATT">
        <xsl:with-param name="ci" select="./(* | @*)" />
      </xsl:call-template>
    </product>
  </xsl:template>
  <!--End of: PSMClass: "Product"-->
  <xsl:template name="TOP-Purchase-Items-Item-Product-ATT">
    <xsl:param name="ci" as="item()*" />
    <!--ref PSMAttribute: "Product.code" 1..1-->
    <xsl:apply-templates select="$ci[name() = 'code']" />
    <!--ref PSMAttribute: "Product.subcode" 1..1-->
    <xsl:apply-templates select="$ci[name() = 'subcode']" />
    <!--ref PSMAttribute: "Product.title" 1..1-->
    <xsl:apply-templates select="$ci[name() = 'title']" />
  </xsl:template>
  <!--PSMClass: "ItemI"-->
  <xsl:template match="/purchase-request/item-info">
    <item-info>
      <xsl:call-template name="TOP-Purchase-Items-Item-ItemI-ATT">
        <xsl:with-param name="ci" select="./(* | @*)" />
      </xsl:call-template>
      <xsl:call-template name="TOP-Purchase-Items-Item-ItemI-ELM">
        <xsl:with-param name="ci" select="./(* | @*)" />
      </xsl:call-template>
    </item-info>
  </xsl:template>
  <!--End of: PSMClass: "ItemI"-->
  <xsl:template name="TOP-Purchase-Items-Item-ItemI-ATT">
    <xsl:param name="ci" as="item()*" />
    <!--ref PSMAttribute: "ItemI.amount" 1..1-->
    <xsl:apply-templates select="$ci[name() = 'amount']" />
  </xsl:template>
  <xsl:template name="TOP-Purchase-Items-Item-ItemI-ELM">
    <xsl:param name="ci" as="item()*" />
    <!--ref PSMAttribute: "ItemI.unit-price" 1..1-->
    <xsl:apply-templates select="$ci[name() = 'unit-price']" />
  </xsl:template>
  <!-- Instance generators -->
  <xsl:template name="TOP-Purchase-SalesAssistant-emp-no-IG">
    <xsl:param name="count" as="item()" select="1" />
    <xsl:for-each select="1 to $count">
      <emp-no>emp-no<xsl:value-of select="current()" /></emp-no>
    </xsl:for-each>
  </xsl:template>
  <xsl:template name="TOP-Purchase-SalesAssistant-name-IG">
    <xsl:param name="count" as="item()" select="1" />
    <xsl:for-each select="1 to $count">
      <name>name<xsl:value-of select="current()" /></name>
    </xsl:for-each>
  </xsl:template>
  <xsl:template name="TOP-Purchase-CustomerInfo-Customer-Address-IG">
    <xsl:param name="count" as="item()" select="1" />
    <xsl:for-each select="1 to $count">
      <delivery-address>
        <xsl:call-template name="TOP-Purchase-CustomerInfo-Customer-Address-ELM-IG" />
      </delivery-address>
    </xsl:for-each>
  </xsl:template>
  <xsl:template name="TOP-Purchase-CustomerInfo-Customer-Address-ELM-IG">
    <xsl:param name="count" as="item()" select="1" />
    <xsl:for-each select="1 to $count">
      <postcode>postcode</postcode>
      <city>city</city>
      <street>street</street>
    </xsl:for-each>
  </xsl:template>
  <xsl:template name="TOP-Purchase-CustomerInfo-Customer-CustEmail-email-IG">
    <xsl:param name="count" as="item()" select="1" />
    <xsl:for-each select="1 to $count">
      <email>email<xsl:value-of select="current()" /></email>
    </xsl:for-each>
  </xsl:template>
  <xsl:template name="TOP-Purchase-Items-Item-ELM-IG">
    <xsl:param name="count" as="item()" select="1" />
    <xsl:for-each select="1 to $count">
      <xsl:call-template name="TOP-Purchase-Items-Item-Product-IG" />
      <xsl:call-template name="TOP-Purchase-Items-Item-ItemI-IG" />
    </xsl:for-each>
  </xsl:template>
  <xsl:template name="TOP-Purchase-Items-Item-Product-IG">
    <xsl:param name="count" as="item()" select="1" />
    <xsl:for-each select="1 to $count">
      <product>
        <xsl:call-template name="TOP-Purchase-Items-Item-Product-ATT-IG" />
      </product>
    </xsl:for-each>
  </xsl:template>
  <xsl:template name="TOP-Purchase-Items-Item-Product-ATT-IG">
    <xsl:param name="count" as="item()" select="1" />
    <xsl:for-each select="1 to $count">
      <xsl:attribute name="code">code</xsl:attribute>
      <xsl:attribute name="subcode">subcode</xsl:attribute>
      <xsl:attribute name="title">title</xsl:attribute>
    </xsl:for-each>
  </xsl:template>
  <xsl:template name="TOP-Purchase-Items-Item-ItemI-IG">
    <xsl:param name="count" as="item()" select="1" />
    <xsl:for-each select="1 to $count">
      <item-info>
        <xsl:call-template name="TOP-Purchase-Items-Item-ItemI-ATT-IG" />
        <xsl:call-template name="TOP-Purchase-Items-Item-ItemI-ELM-IG" />
      </item-info>
    </xsl:for-each>
  </xsl:template>
  <xsl:template name="TOP-Purchase-Items-Item-ItemI-ATT-IG">
    <xsl:param name="count" as="item()" select="1" />
    <xsl:for-each select="1 to $count">
      <xsl:attribute name="amount">amount</xsl:attribute>
    </xsl:for-each>
  </xsl:template>
  <xsl:template name="TOP-Purchase-Items-Item-ItemI-ELM-IG">
    <xsl:param name="count" as="item()" select="1" />
    <xsl:for-each select="1 to $count">
      <unit-price>unit-price</unit-price>
    </xsl:for-each>
  </xsl:template>
  <!--Element to attribute conversion template-->
  <xsl:template match="title | amount" priority="0">
    <xsl:attribute name="{name()}">
      <xsl:value-of select="." />
    </xsl:attribute>
  </xsl:template>
  <!--Attribute to element conversion template-->
  <xsl:template match="@emp-no" priority="0">
    <xsl:element name="{name()}">
      <xsl:value-of select="." />
    </xsl:element>
  </xsl:template>
  <!--No blue nodes-->
  <!--Green nodes template-->
  <xsl:template match="@purchase-date | @customer-no | postcode | city | street | @code | @subcode | unit-price | email" priority="0">
    <xsl:copy-of select="." />
  </xsl:template>
</xsl:stylesheet>