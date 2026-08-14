export interface Company {
  id: string;
  name: string;
  ticker: string;
  sector: string;
  marketCap: number;
}

export interface CompanySummary {
  id: string;
  name: string;
  ticker: string;
  sector: string;
  marketCap: number;
}

export interface CompanyDetailResponse {
  id: string;
  name: string;
  ticker: string;
  sector: string;
  marketCap: number;
  directorCount: number;
  maxSupplyDependencyPct: number;
  institutionCount: number;
}

export interface DbHealthResponse {
  status: string;
  result: number;
}
