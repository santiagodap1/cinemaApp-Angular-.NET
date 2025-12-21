export interface Address {
  street: string;
  city: string;
  state: string;
  country: string;
  postalCode: string;
}

export interface CinemaSite {
  id: string;
  name: string;
  address: Address;
}

export interface CreateCinemaSiteRequest {
  name: string;
  address: Address;
}

export interface UpdateCinemaSiteRequest {
  name: string;
  address: Address;
}
