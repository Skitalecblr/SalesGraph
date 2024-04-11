import { Component, Input, OnInit } from '@angular/core';
import { SaleService } from '../../../../services/sales.service';
import { SaleList, SaleListItem } from '../../../../viewModels/sale/sale-list.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-sale-list',
  templateUrl: './sale-list.component.html',
  styleUrls: ['./sale-list.component.css']
})
export class SaleListComponent implements OnInit {

  @Input() productId: string = '';

  saleList: SaleList = { sales: [], totalPages: 0 };
  pageSize: number = 10;
  currentPage: number = 1;
  loading: boolean = false;
  editingSaleItem: SaleListItem = <SaleListItem>{};
  isEditing: boolean = false;
  saleForm: FormGroup;

  constructor(private saleService: SaleService, private formBuilder: FormBuilder) {
    
    this.saleForm = this.formBuilder.group({
      saleDate: [null, Validators.required],
      quantity: [null, Validators.required]
    });
  }

  ngOnInit(): void {
    this.getSales();
  }

  getSales(): void {
    this.loading = true;
    this.saleService.getSalesForProduct(this.productId, this.pageSize, this.currentPage)
      .subscribe((result: SaleList) => {
        this.saleList = result;
        this.loading = false;
      });
  }

  onPageChange(pageNumber: number): void {
    this.currentPage = pageNumber;
    this.getSales();
  }

  getDefaultSale() {
    let obj: SaleListItem = {
      id: "",
      productId: this.productId,
      saleDate: new Date(),
      quantity: 1,
    };

    return obj;
  }

  startEditing(saleItem: SaleListItem): void {
    this.editingSaleItem.id = saleItem.id;
    this.editingSaleItem.quantity = saleItem.quantity;
    this.editingSaleItem.saleDate = saleItem.saleDate;
    this.editingSaleItem.productId = saleItem.productId;
    this.saleForm.patchValue({
      saleDate: this.editingSaleItem.saleDate,
      quantity: this.editingSaleItem.quantity
    });
    this.isEditing = true;
  }

  cancelEdit(): void {
    this.editingSaleItem = this.getDefaultSale();
    this.isEditing = false;
    this.saleForm.reset();
    this.getSales();
  }

  saveSaleItem(): void {
    if (this.editingSaleItem.id) {
      
      this.saleService.updateSaleItem(this.editingSaleItem).subscribe(() => {
        console.log('Sale item updated:', this.editingSaleItem);
        this.cancelEdit();
      });
    } else {
      
      this.saleService.createSaleItem(this.editingSaleItem).subscribe(() => {
        console.log('Sale item created:', this.editingSaleItem);
        this.cancelEdit();
      });
    }
  }

  deleteSaleItem(saleItemId: string): void {
    if (confirm('Are you sure you want to delete this sale item?')) {
      this.saleService.deleteSaleItem(saleItemId).subscribe(() => {
        console.log('Sale item deleted:', saleItemId);
        this.getSales();
      });
    }
  }
}
