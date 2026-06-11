import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Contact } from '../../../core/models/contact.model';
import { ContactService } from '../../../core/services/contact.service';
import { ConfirmModalComponent } from '../../../shared/confirm-modal/confirm-modal.component';

@Component({
  selector: 'app-contact-detail',
  imports: [RouterLink],
  templateUrl: './contact-detail.component.html',
})
export class ContactDetailComponent implements OnInit {
  private readonly contactService = inject(ContactService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly modalService = inject(NgbModal);

  contact?: Contact;

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id')!;

    this.contactService.getContact(id).subscribe({
      next: (contact) => (this.contact = contact),
      error: () => this.router.navigate(['/contacts']),
    });
  }

  confirmDelete(contact: Contact): void {
    const modalRef = this.modalService.open(ConfirmModalComponent);
    modalRef.componentInstance.title = 'Delete contact';
    modalRef.componentInstance.message = `Are you sure you want to delete ${contact.firstName} ${contact.lastName}?`;
    modalRef.componentInstance.confirmLabel = 'Delete';

    modalRef.closed.subscribe(() => {
      this.contactService.deleteContact(contact.id).subscribe(() => this.router.navigate(['/contacts']));
    });
  }
}
