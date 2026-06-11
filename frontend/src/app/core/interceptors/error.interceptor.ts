import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { ErrorDialogService } from '../services/error-dialog.service';

/** Statuses handled inline by forms (validation and duplicate-phone conflicts). */
const FORM_HANDLED_STATUSES = [400, 409];

/**
 * Shows server errors to the user in a modal, then rethrows so callers can still react.
 */
export const errorInterceptor: HttpInterceptorFn = (request, next) => {
  const errorDialog = inject(ErrorDialogService);

  return next(request).pipe(
    catchError((error: unknown) => {
      if (error instanceof HttpErrorResponse && !FORM_HANDLED_STATUSES.includes(error.status)) {
        errorDialog.show(error);
      }

      return throwError(() => error);
    }),
  );
};
