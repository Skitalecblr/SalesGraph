import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse, HttpEvent } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class BaseHttpService {

  baseUrl: string = "";

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.baseUrl = baseUrl;
  }

  private handleHttpError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong.
      console.error(
        `Backend returned code ${error.status}, ` +
        `body was: ${error.error}`);
    }
    // Return an observable with a user-facing error message.
    return throwError('Something bad happened; please try again later.');
  }

  get<T>(url: string, options?: object): Observable<T> {
    return this.http.get<T>(this.baseUrl + url, options)
      .pipe(
        catchError(this.handleHttpError)
      );
  }

  post<T>(url: string, data: any, options?: any): Observable<HttpEvent<T>> {
    return this.http.post<T>(this.baseUrl + url, data, options)
      .pipe(
        catchError(this.handleHttpError)
      );
  }

  put<T>(url: string, data: any, options?: any): Observable<HttpEvent<T>> {
    return this.http.put<T>(this.baseUrl + url, data, options)
      .pipe(
        catchError(this.handleHttpError)
      );
  }

  delete<T>(url: string, options?: any): Observable<HttpEvent<T>> {
    return this.http.delete<T>(this.baseUrl + url, options)
      .pipe(
        catchError(this.handleHttpError)
      );
  }
}
