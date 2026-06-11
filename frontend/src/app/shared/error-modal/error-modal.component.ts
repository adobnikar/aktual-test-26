import { Component, inject } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-error-modal',
  templateUrl: './error-modal.component.html',
})
export class ErrorModalComponent {
  readonly activeModal = inject(NgbActiveModal);

  title = 'Error';
  message = 'An unexpected error occurred.';
}
