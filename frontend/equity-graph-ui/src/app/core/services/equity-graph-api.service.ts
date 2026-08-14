import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CompanySummary, CompanyDetailResponse, DbHealthResponse } from '../models/company';
import { BoardInterlock } from '../models/board-interlock';
import { InstitutionalOverlapEntry } from '../models/institutional-overlap';
import { SupplyChainPath } from '../models/supply-chain-path';
import { ShortestPathResponse } from '../models/shortest-path';

@Injectable({
  providedIn: 'root'
})
export class EquityGraphApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiBaseUrl;

  /**
   * 1. GET /api/companies?search=&sector=
   * Fetch all companies or search/filter by name/ticker and sector.
   */
  getCompanies(search?: string, sector?: string): Observable<CompanySummary[]> {
    let params = new HttpParams();
    if (search) {
      params = params.set('search', search);
    }
    if (sector) {
      params = params.set('sector', sector);
    }
    return this.http.get<CompanySummary[]>(`${this.baseUrl}/companies`, { params });
  }

  /**
   * 2. GET /api/companies/{id}
   * Fetch detailed company info with aggregated graph metrics.
   */
  getCompanyById(id: string): Observable<CompanyDetailResponse> {
    return this.http.get<CompanyDetailResponse>(`${this.baseUrl}/companies/${id}`);
  }

  /**
   * 3. GET /api/companies/{id}/board-interlocks
   * Fetch shared board director relationships for a given company.
   */
  getBoardInterlocks(id: string): Observable<BoardInterlock[]> {
    return this.http.get<BoardInterlock[]>(`${this.baseUrl}/companies/${id}/board-interlocks`);
  }

  /**
   * 4. GET /api/companies/overlap?companyIdA=&companyIdB=
   * Fetch common institutional shareholders and stake percentages between two companies.
   */
  getInstitutionalOverlap(companyIdA: string, companyIdB: string): Observable<InstitutionalOverlapEntry[]> {
    const params = new HttpParams()
      .set('companyIdA', companyIdA)
      .set('companyIdB', companyIdB);
    return this.http.get<InstitutionalOverlapEntry[]>(`${this.baseUrl}/companies/overlap`, { params });
  }

  /**
   * 5. GET /api/companies/{id}/supply-chain-exposure?maxHops=
   * Fetch multi-hop upstream/downstream supply chain dependency paths.
   */
  getSupplyChainExposure(id: string, maxHops?: number): Observable<SupplyChainPath[]> {
    let params = new HttpParams();
    if (maxHops !== undefined && maxHops !== null) {
      params = params.set('maxHops', maxHops.toString());
    }
    return this.http.get<SupplyChainPath[]>(`${this.baseUrl}/companies/${id}/supply-chain-exposure`, { params });
  }

  /**
   * 6. GET /api/companies/shortest-path?fromCompanyId=&toCompanyId=
   * Discover shortest connection path across all relationship types between two companies.
   */
  getShortestPath(fromCompanyId: string, toCompanyId: string): Observable<ShortestPathResponse> {
    const params = new HttpParams()
      .set('fromCompanyId', fromCompanyId)
      .set('toCompanyId', toCompanyId);
    return this.http.get<ShortestPathResponse>(`${this.baseUrl}/companies/shortest-path`, { params });
  }

  /**
   * 7. GET /api/health/db
   * Verify graph database connectivity and health.
   */
  checkDbHealth(): Observable<DbHealthResponse> {
    return this.http.get<DbHealthResponse>(`${this.baseUrl}/health/db`);
  }
}
