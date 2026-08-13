// ==========================================
// EquityGraph Seed Data
// Uses MERGE for idempotent, safe execution
// ==========================================

// 1. Company Nodes (8 companies across 4 sectors)
MERGE (c1:Company {id: 'comp-1'})
SET c1.name = 'Tata Consultancy Services', c1.ticker = 'TCS.NS', c1.sector = 'Information Technology', c1.marketCap = 160000000000.0;

MERGE (c2:Company {id: 'comp-2'})
SET c2.name = 'Infosys Limited', c2.ticker = 'INFY.NS', c2.sector = 'Information Technology', c2.marketCap = 80000000000.0;

MERGE (c3:Company {id: 'comp-3'})
SET c3.name = 'Tata Motors Limited', c3.ticker = 'TATAMOTORS.NS', c3.sector = 'Automotive', c3.marketCap = 42000000000.0;

MERGE (c4:Company {id: 'comp-4'})
SET c4.name = 'Mahindra & Mahindra Limited', c4.ticker = 'M&M.NS', c4.sector = 'Automotive', c4.marketCap = 38000000000.0;

MERGE (c5:Company {id: 'comp-5'})
SET c5.name = 'HDFC Bank Limited', c5.ticker = 'HDFCBANK.NS', c5.sector = 'Financial Services', c5.marketCap = 145000000000.0;

MERGE (c6:Company {id: 'comp-6'})
SET c6.name = 'ICICI Bank Limited', c6.ticker = 'ICICIBANK.NS', c6.sector = 'Financial Services', c6.marketCap = 95000000000.0;

MERGE (c7:Company {id: 'comp-7'})
SET c7.name = 'Bosch Limited', c7.ticker = 'BOSCHLTD.NS', c7.sector = 'Automotive Components', c7.marketCap = 11000000000.0;

MERGE (c8:Company {id: 'comp-8'})
SET c8.name = 'ASM Technologies', c8.ticker = 'ASMTEC.NS', c8.sector = 'Semiconductors', c8.marketCap = 1500000000.0;


// 2. Person Nodes (11 Directors)
MERGE (p1:Person {id: 'person-1'}) SET p1.name = 'Natarajan Chandrasekaran';
MERGE (p2:Person {id: 'person-2'}) SET p2.name = 'Keki Mistry';
MERGE (p3:Person {id: 'person-3'}) SET p3.name = 'Anand Mahindra';
MERGE (p4:Person {id: 'person-4'}) SET p4.name = 'Salil Parekh';
MERGE (p5:Person {id: 'person-5'}) SET p5.name = 'Nandan Nilekani';
MERGE (p6:Person {id: 'person-6'}) SET p6.name = 'Sashidhar Jagdishan';
MERGE (p7:Person {id: 'person-7'}) SET p7.name = 'Sandeep Bakhshi';
MERGE (p8:Person {id: 'person-8'}) SET p8.name = 'Soumitra Bhattacharya';
MERGE (p9:Person {id: 'person-9'}) SET p9.name = 'Rabindra Srikantan';
MERGE (p10:Person {id: 'person-10'}) SET p10.name = 'Girish Wagh';
MERGE (p11:Person {id: 'person-11'}) SET p11.name = 'Ireena Vittal';


// 3. DIRECTOR_OF Relationships (with overlaps across multiple boards)
MERGE (p1:Person {id: 'person-1'})
MERGE (c1:Company {id: 'comp-1'})
MERGE (p1)-[r1:DIRECTOR_OF]->(c1)
SET r1.since = 2016;

MERGE (p1:Person {id: 'person-1'})
MERGE (c3:Company {id: 'comp-3'})
MERGE (p1)-[r2:DIRECTOR_OF]->(c3)
SET r2.since = 2017;

MERGE (p2:Person {id: 'person-2'})
MERGE (c1:Company {id: 'comp-1'})
MERGE (p2)-[r3:DIRECTOR_OF]->(c1)
SET r3.since = 2018;

MERGE (p2:Person {id: 'person-2'})
MERGE (c5:Company {id: 'comp-5'})
MERGE (p2)-[r4:DIRECTOR_OF]->(c5)
SET r4.since = 2019;

MERGE (p3:Person {id: 'person-3'})
MERGE (c4:Company {id: 'comp-4'})
MERGE (p3)-[r5:DIRECTOR_OF]->(c4)
SET r5.since = 2003;

MERGE (p3:Person {id: 'person-3'})
MERGE (c6:Company {id: 'comp-6'})
MERGE (p3)-[r6:DIRECTOR_OF]->(c6)
SET r6.since = 2020;

MERGE (p4:Person {id: 'person-4'})
MERGE (c2:Company {id: 'comp-2'})
MERGE (p4)-[r7:DIRECTOR_OF]->(c2)
SET r7.since = 2018;

MERGE (p5:Person {id: 'person-5'})
MERGE (c2:Company {id: 'comp-2'})
MERGE (p5)-[r8:DIRECTOR_OF]->(c2)
SET r8.since = 2017;

MERGE (p6:Person {id: 'person-6'})
MERGE (c5:Company {id: 'comp-5'})
MERGE (p6)-[r9:DIRECTOR_OF]->(c5)
SET r9.since = 2020;

MERGE (p7:Person {id: 'person-7'})
MERGE (c6:Company {id: 'comp-6'})
MERGE (p7)-[r10:DIRECTOR_OF]->(c6)
SET r10.since = 2018;

MERGE (p8:Person {id: 'person-8'})
MERGE (c7:Company {id: 'comp-7'})
MERGE (p8)-[r11:DIRECTOR_OF]->(c7)
SET r11.since = 2017;

MERGE (p8:Person {id: 'person-8'})
MERGE (c4:Company {id: 'comp-4'})
MERGE (p8)-[r12:DIRECTOR_OF]->(c4)
SET r12.since = 2021;

MERGE (p9:Person {id: 'person-9'})
MERGE (c8:Company {id: 'comp-8'})
MERGE (p9)-[r13:DIRECTOR_OF]->(c8)
SET r13.since = 2015;

MERGE (p10:Person {id: 'person-10'})
MERGE (c3:Company {id: 'comp-3'})
MERGE (p10)-[r14:DIRECTOR_OF]->(c3)
SET r14.since = 2021;

MERGE (p11:Person {id: 'person-11'})
MERGE (c1:Company {id: 'comp-1'})
MERGE (p11)-[r15:DIRECTOR_OF]->(c1)
SET r15.since = 2022;

MERGE (p11:Person {id: 'person-11'})
MERGE (c5:Company {id: 'comp-5'})
MERGE (p11)-[r16:DIRECTOR_OF]->(c5)
SET r16.since = 2021;


// 4. SUPPLIES_TO Relationships (forming multi-hop supply chains)
// Chain 1: ASM Technologies (c8) -> Bosch (c7) -> Tata Motors (c3) -> TCS (c1) -> HDFC Bank (c5)
MERGE (c8:Company {id: 'comp-8'})
MERGE (c7:Company {id: 'comp-7'})
MERGE (c8)-[s1:SUPPLIES_TO]->(c7)
SET s1.dependencyPct = 35.0;

MERGE (c7:Company {id: 'comp-7'})
MERGE (c3:Company {id: 'comp-3'})
MERGE (c7)-[s2:SUPPLIES_TO]->(c3)
SET s2.dependencyPct = 28.5;

MERGE (c3:Company {id: 'comp-3'})
MERGE (c1:Company {id: 'comp-1'})
MERGE (c3)-[s3:SUPPLIES_TO]->(c1)
SET s3.dependencyPct = 15.0;

MERGE (c1:Company {id: 'comp-1'})
MERGE (c5:Company {id: 'comp-5'})
MERGE (c1)-[s4:SUPPLIES_TO]->(c5)
SET s4.dependencyPct = 18.0;

// Chain 2: Bosch (c7) -> Mahindra & Mahindra (c4)
MERGE (c7:Company {id: 'comp-7'})
MERGE (c4:Company {id: 'comp-4'})
MERGE (c7)-[s5:SUPPLIES_TO]->(c4)
SET s5.dependencyPct = 22.0;

// Chain 3: ASM Technologies (c8) -> Infosys (c2) -> ICICI Bank (c6)
MERGE (c8:Company {id: 'comp-8'})
MERGE (c2:Company {id: 'comp-2'})
MERGE (c8)-[s6:SUPPLIES_TO]->(c2)
SET s6.dependencyPct = 12.0;

MERGE (c2:Company {id: 'comp-2'})
MERGE (c6:Company {id: 'comp-6'})
MERGE (c2)-[s7:SUPPLIES_TO]->(c6)
SET s7.dependencyPct = 20.0;


// 5. Institution Nodes (6 Institutions)
MERGE (i1:Institution {id: 'inst-1'}) SET i1.name = 'Life Insurance Corporation of India';
MERGE (i2:Institution {id: 'inst-2'}) SET i2.name = 'Vanguard Group';
MERGE (i3:Institution {id: 'inst-3'}) SET i3.name = 'BlackRock Inc.';
MERGE (i4:Institution {id: 'inst-4'}) SET i4.name = 'SBI Mutual Fund';
MERGE (i5:Institution {id: 'inst-5'}) SET i5.name = 'ICICI Prudential AMC';
MERGE (i6:Institution {id: 'inst-6'}) SET i6.name = 'GIC Private Limited';


// 6. HOLDS_STAKE_IN Relationships (with overlaps across companies)
// LIC Holdings
MERGE (i1:Institution {id: 'inst-1'})
MERGE (c1:Company {id: 'comp-1'})
MERGE (i1)-[h1:HOLDS_STAKE_IN]->(c1)
SET h1.stakePct = 4.8;

MERGE (i1:Institution {id: 'inst-1'})
MERGE (c2:Company {id: 'comp-2'})
MERGE (i1)-[h2:HOLDS_STAKE_IN]->(c2)
SET h2.stakePct = 7.2;

MERGE (i1:Institution {id: 'inst-1'})
MERGE (c3:Company {id: 'comp-3'})
MERGE (i1)-[h3:HOLDS_STAKE_IN]->(c3)
SET h3.stakePct = 5.4;

MERGE (i1:Institution {id: 'inst-1'})
MERGE (c5:Company {id: 'comp-5'})
MERGE (i1)-[h4:HOLDS_STAKE_IN]->(c5)
SET h4.stakePct = 5.1;

// Vanguard Holdings
MERGE (i2:Institution {id: 'inst-2'})
MERGE (c1:Company {id: 'comp-1'})
MERGE (i2)-[h5:HOLDS_STAKE_IN]->(c1)
SET h5.stakePct = 3.5;

MERGE (i2:Institution {id: 'inst-2'})
MERGE (c2:Company {id: 'comp-2'})
MERGE (i2)-[h6:HOLDS_STAKE_IN]->(c2)
SET h6.stakePct = 4.1;

MERGE (i2:Institution {id: 'inst-2'})
MERGE (c5:Company {id: 'comp-5'})
MERGE (i2)-[h7:HOLDS_STAKE_IN]->(c5)
SET h7.stakePct = 3.8;

MERGE (i2:Institution {id: 'inst-2'})
MERGE (c6:Company {id: 'comp-6'})
MERGE (i2)-[h8:HOLDS_STAKE_IN]->(c6)
SET h8.stakePct = 3.9;

// BlackRock Holdings
MERGE (i3:Institution {id: 'inst-3'})
MERGE (c1:Company {id: 'comp-1'})
MERGE (i3)-[h9:HOLDS_STAKE_IN]->(c1)
SET h9.stakePct = 3.2;

MERGE (i3:Institution {id: 'inst-3'})
MERGE (c2:Company {id: 'comp-2'})
MERGE (i3)-[h10:HOLDS_STAKE_IN]->(c2)
SET h10.stakePct = 4.6;

MERGE (i3:Institution {id: 'inst-3'})
MERGE (c5:Company {id: 'comp-5'})
MERGE (i3)-[h11:HOLDS_STAKE_IN]->(c5)
SET h11.stakePct = 4.0;

MERGE (i3:Institution {id: 'inst-3'})
MERGE (c6:Company {id: 'comp-6'})
MERGE (i3)-[h12:HOLDS_STAKE_IN]->(c6)
SET h12.stakePct = 3.7;

// SBI Mutual Fund Holdings
MERGE (i4:Institution {id: 'inst-4'})
MERGE (c3:Company {id: 'comp-3'})
MERGE (i4)-[h13:HOLDS_STAKE_IN]->(c3)
SET h13.stakePct = 3.1;

MERGE (i4:Institution {id: 'inst-4'})
MERGE (c4:Company {id: 'comp-4'})
MERGE (i4)-[h14:HOLDS_STAKE_IN]->(c4)
SET h14.stakePct = 4.5;

MERGE (i4:Institution {id: 'inst-4'})
MERGE (c6:Company {id: 'comp-6'})
MERGE (i4)-[h15:HOLDS_STAKE_IN]->(c6)
SET h15.stakePct = 5.2;

// ICICI Prudential AMC Holdings
MERGE (i5:Institution {id: 'inst-5'})
MERGE (c3:Company {id: 'comp-3'})
MERGE (i5)-[h16:HOLDS_STAKE_IN]->(c3)
SET h16.stakePct = 2.8;

MERGE (i5:Institution {id: 'inst-5'})
MERGE (c4:Company {id: 'comp-4'})
MERGE (i5)-[h17:HOLDS_STAKE_IN]->(c4)
SET h17.stakePct = 3.6;

MERGE (i5:Institution {id: 'inst-5'})
MERGE (c7:Company {id: 'comp-7'})
MERGE (i5)-[h18:HOLDS_STAKE_IN]->(c7)
SET h18.stakePct = 4.2;

// GIC Private Limited Holdings
MERGE (i6:Institution {id: 'inst-6'})
MERGE (c5:Company {id: 'comp-5'})
MERGE (i6)-[h19:HOLDS_STAKE_IN]->(c5)
SET h19.stakePct = 2.4;

MERGE (i6:Institution {id: 'inst-6'})
MERGE (c6:Company {id: 'comp-6'})
MERGE (i6)-[h20:HOLDS_STAKE_IN]->(c6)
SET h20.stakePct = 2.9;
