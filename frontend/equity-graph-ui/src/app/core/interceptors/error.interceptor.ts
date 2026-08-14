import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';

export interface ApiError {
  status: number;
  message: string;
}

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      const normalizedError: ApiError = {
        status: error.status,
        message: error.error?.message || error.message || 'An unexpected error occurred'
      };

      // TODO: Add UI error notification/toast surfacing logic here

      return throwError(() => normalizedError);
    })
  );
};
