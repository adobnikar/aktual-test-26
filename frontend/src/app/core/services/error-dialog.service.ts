import { HttpErrorResponse } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ErrorModalComponent } from '../../shared/error-modal/error-modal.component';

@Injectable({ providedIn: 'root' })
export class ErrorDialogService {
  private readonly modalService = inject(NgbModal);

  private open = false;

  /** Shows a server error in a modal; only one error modal is shown at a time. */
  show(error: HttpErrorResponse): void {
    if (this.open) {
      return;
    }

    this.open = true;

    const modalRef = this.modalService.open(ErrorModalComponent);
    modalRef.componentInstance.title = this.titleFor(error);
    modalRef.componentInstance.message = this.messageFor(error);

    modalRef.hidden.subscribe(() => (this.open = false));
  }

  private titleFor(error: HttpErrorResponse): string {
    if (error.status === 0) {
      return 'Connection error';
    }

    // RFC 7807 problem details returned by the backend.
    return error.error?.title ?? 'Server error';
  }

  private messageFor(error: HttpErrorResponse): string {
    if (error.status === 0) {
      return 'The server could not be reached. Please check your connection and try again.';
    }

    return error.error?.detail ?? 'An unexpected server error occurred. Please try again.';
  }
}
