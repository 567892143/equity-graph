import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';

export interface ApiError {
  status: number;
  message: string;
}

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let message: string;

      switch (error.status) {
        case 0:
          message = 'Cannot reach the server. Is the API running?';
          break;
        case 404:
          message = error.error?.message || error.message || 'Resource not found';
          break;
        case 400:
          message = error.error?.message || error.message || 'Bad request';
          break;
        case 503:
          message = 'Database is temporarily unavailable. Please try again shortly.';
          break;
        default:
          message = error.error?.message || error.message || 'An unexpected error occurred';
          break;
      }

      const normalizedError: ApiError = {
        status: error.status,
        message
      };

      // TODO: Future UI error-surfacing / toast notification logic will consume normalizedError

      return throwError(() => normalizedError);
    })
  );
};
