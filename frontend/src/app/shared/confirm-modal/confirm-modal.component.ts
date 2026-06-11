import { Component, inject } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

/**
 * Generic confirmation dialog; closes with true on confirm, dismisses on cancel.
 * Open via NgbModal and set title/message/confirmLabel on the component instance.
 */
@Component({
  selector: 'app-confirm-modal',
  templateUrl: './confirm-modal.component.html',
})
export class ConfirmModalComponent {
  readonly activeModal = inject(NgbActiveModal);

  title = 'Confirm';
  message = 'Are you sure?';
  confirmLabel = 'Confirm';
}
