export interface Contact {
  id: string;
  firstName: string;
  lastName: string;
  address: string;
  phoneNumber: string;
}

/** Payload for creating or updating a contact. */
export type ContactRequest = Omit<Contact, 'id'>;
