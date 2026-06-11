import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Contact } from '../../../core/models/contact.model';
import { ContactService } from '../../../core/services/contact.service';

@Component({
  selector: 'app-contact-detail',
  imports: [RouterLink],
  templateUrl: './contact-detail.component.html',
})
export class ContactDetailComponent implements OnInit {
  private readonly contactService = inject(ContactService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  contact?: Contact;

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id')!;

    this.contactService.getContact(id).subscribe({
      next: (contact) => (this.contact = contact),
      error: () => this.router.navigate(['/contacts']),
    });
  }
}
