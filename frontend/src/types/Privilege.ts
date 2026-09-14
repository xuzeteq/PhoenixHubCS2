export type Feature = { id: number, title: string, description: string, icon: string }

export interface Privilege {
    id: number;
    title: string;
    price: number;
    oldPrice: number;
    imageUrl: string;
    features: Feature[]
    createdAt: Date;
}