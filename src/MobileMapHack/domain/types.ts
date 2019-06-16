
export interface Location {
    latitude: number
    longitude: number
}

export interface Journey {
    name: string
    start: Location
    end: Location
}
