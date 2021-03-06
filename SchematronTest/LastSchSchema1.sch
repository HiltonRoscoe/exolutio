<?xml version="1.0" encoding="UTF-8"?><sch:schema xmlns:sch="http://purl.oclc.org/dsdl/schematron">
  <!-- Generated by eXolutio on 16.2.2012 11:31 from tournaments/MatchSchedule. -->
  <sch:pattern id="collections">
    <!--Below follow constraints from OCL script 'collections'. -->
    <sch:rule context="/tournament">
      <!--self.matches.day->Collect(d : Day | d.match)->forAll(m : Match | m.start.after(self.start) and m.end.before(self.end))-->
      <sch:assert test="oclX:forAll(oclX:collect($self/matches/day, function($d) { $d/match }), function($m) { oclDate:after($m/start, $self/start) and oclDate:before($m/end, $self/end) })"/>
    </sch:rule>
    <sch:rule context="/tournament">
      <!--self.matches.day->Collect(d : Day | d.match)->exists(m : Match | m.start = self.start)-->
      <sch:assert test="oclX:exists(oclX:collect($self/matches/day, function($d) { $d/match }), function($m) { $m/start eq $self/start })"/>
    </sch:rule>
    <sch:rule context="/tournament/matches/day/match">
      <!--self.matchPlayers.player->forAll(p : MatchPlayer | p.MatchPlayers.Match.Day.Matches.Tournament.participatingPlayers.player->exists(px : Player | px.name = p.name))-->
      <sch:assert test="oclX:forAll($self/matchPlayers/player, function($p) { oclX:exists($p/../../../../../participatingPlayers/player, function($px) { $px/name eq $p/name }) })"/>
    </sch:rule>
    <sch:rule context="/tournament/participatingPlayers">
      <!--self.player->isUnique(p : Player | p.email)-->
      <sch:assert test="oclX:isUnique($self/player, function($p) { xs:data($p/email) })"/>
    </sch:rule>
  </sch:pattern>
  <sch:pattern id="empty">
    <!--Below follow constraints from OCL script 'empty'. -->
  </sch:pattern>
  <sch:pattern id="days">
    <!--Below follow constraints from OCL script 'days'. -->
    <sch:rule context="/tournament">
      <!--self.matches.day->forAll(d1 : Day, d2 : Day | d1 <> d2 implies d1.date <> d2.date)-->
      <sch:assert test="oclX:forAllN($self/matches/day, function($d1, $d2) { if (not($d1 is $d2)) then $d1/date ne $d2/date else true() })"/>
    </sch:rule>
    <sch:rule context="/tournament">
      <!--self.start <= self.end-->
      <sch:assert test="start le end"/>
    </sch:rule>
  </sch:pattern>
</sch:schema>