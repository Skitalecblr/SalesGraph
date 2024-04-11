export interface SaleListItem {
  id: string;
  productId: string;
  saleDate: Date;
  quantity: number;
}

export interface SaleList {
  sales: SaleListItem[];
  totalPages: number;
}
