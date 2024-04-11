import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ProductDetail } from '../viewModels/product/product-detail.model';
import { ProductList } from '../viewModels/product/product-list.model';
import { BaseHttpService } from './base.serivce';

@Injectable({
  providedIn: 'root'
})

export class ProductService extends BaseHttpService {

  private url: string = "product";
   
  getProductList(): Observable<ProductList> {
    return this.get<ProductList>(this.url);
  }

  getProductDetails(productId: string): Observable<ProductDetail> {
    return this.get<ProductDetail>(`${this.url}/${productId}`);
  }
}
