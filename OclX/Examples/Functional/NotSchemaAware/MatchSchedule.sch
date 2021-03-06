﻿<?xml version="1.0" encoding="utf-8"?>
<sch:schema xmlns:sch="http://purl.oclc.org/dsdl/schematron">
  <!-- Generated by eXolutio on 9.4.2012 18:51 from tournaments/MatchSchedule. -->
  <!--Below follow constraints from OCL script 'collections'. -->
  <sch:pattern id="Tournament">
    <sch:rule context="/tournament">
      <sch:let name="self" value="." />
      <!--self.start <= self.end-->
      <sch:assert test="xs:dateTime(start) le xs:dateTime(end)">Dates inconsistent, <sch:value-of select="xs:dateTime($self/start)" /> is greater than <sch:value-of select="xs:dateTime($self/end)" /> in <sch:value-of select="$self/name" /></sch:assert>
      <!--self.matches.day->collect(d : Day | d.match)->forAll(m : Match | m.start >= self.start and m.end <= self.end)-->
      <sch:assert test="oclX:forAll(oclX:collect(matches/day, function($d) { $d/match }), function($m) { xs:dateTime($m/start) ge xs:dateTime($self/start) and xs:dateTime($m/end) le xs:dateTime($self/end) })">All matches in a tournament occur within the tournament's time frame</sch:assert>
      <!--self.matches.day->collect(d : Day | d.match)->exists(m : Match | m.start.trunc() = self.start.trunc())-->
      <sch:assert test="oclX:exists(oclX:collect(matches/day, function($d) { $d/match }), function($m) { oclDate:trunc(xs:dateTime($m/start)) eq oclDate:trunc(xs:dateTime($self/start)) })">Each tournament conducts at least one match on the first day of the tournament</sch:assert>
      <!--self.matches.day->forAll(d1 : Day, d2 : Day | d1 <> d2 implies d1.date <> d2.date)-->
      <sch:assert test="oclX:forAllN(matches/day, function($d1, $d2) { if (not($d1 is $d2)) then xs:date($d1/date) ne xs:date($d2/date) else true() })">Days must be unique</sch:assert>
    </sch:rule>
  </sch:pattern>
  <sch:pattern id="Match">
    <sch:rule context="/tournament/matches/day/match">
      <sch:let name="self" value="." />
      <!--self.matchPlayers.player->forAll(p : MatchPlayer | p.MatchPlayers.Match.Day.Matches.Tournament.participatingPlayers.player->exists(px : Player | px.name = p.name))-->
      <sch:assert test="oclX:forAll(matchPlayers/player, function($p) { oclX:exists($p/../../../../../participatingPlayers/player, function($px) { $px/name eq $p/name }) })">A match can only involve players who are accepted in the tournament</sch:assert>
    </sch:rule>
  </sch:pattern>
  <sch:pattern id="Players">
    <sch:rule context="/tournament/participatingPlayers">
      <sch:let name="self" value="." />
      <!--self.player->isUnique(p : Player | p.email)-->
      <sch:assert test="oclX:isUnique(player, function($p) { data($p/email) })">Players must have unique email</sch:assert>
    </sch:rule>
  </sch:pattern>
  <sch:pattern id="Day">
    <sch:rule context="/tournament/matches/day">
      <sch:let name="self" value="." />
      <!--self.match->forAll(m : Match | m.start.getDate() = self.date)-->
      <sch:assert test="oclX:forAll(match, function($m) { oclDate:getDate(xs:dateTime($m/start)) eq xs:date($self/date) })">Each day shows only matches taking place that day</sch:assert>
    </sch:rule>
  </sch:pattern>
  <!--Below follow constraints from OCL script 'empty'. -->
</sch:schema>