
export interface LatLng {
    latitude: number
    longitude: number
}

export interface Polyline {
    coordinates: LatLng[]
}

export interface MapData {
    lines: Polyline[]
}

export interface Journey {
    name: string
    start: Location
    end: Location
}
