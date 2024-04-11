import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SaleList, SaleListItem } from '../viewModels/sale/sale-list.model';
import { BaseHttpService } from './base.serivce';

@Injectable({
  providedIn: 'root'
})

export class SaleService extends BaseHttpService {

  private url: string = "sale";
   
  getSalesForProduct(productId: string, pageSize: number, pageNumber: number): Observable<SaleList> {
    return this.get<SaleList>(`${this.url}?productId=${productId}&pageSize=${pageSize.toString() }&pageNumber=${pageNumber.toString()}`);
  }

  createSaleItem(saleItem: SaleListItem): Observable<any> {
    return this.post<any>(`${this.url}`, saleItem);
  }

  updateSaleItem(saleItem: SaleListItem): Observable<any> {
    return this.put<any>(`${this.url}`, saleItem);
  }

  deleteSaleItem(saleItemId: string): Observable<any> {
    return this.delete<any>(`${this.url}/${saleItemId}`);
  }

  getSalesStats(period: number, startDate: string, endDate: string): Observable<any> {
    
    return this.get<any>(`${this.url}/stats?period=${period}&startDate=${startDate}&endDate=${endDate}`);
  }

}
