
export interface LatLng {
    latitude: number
    longitude: number
}

export interface Polyline {
    coordinates: LatLng[]
    strokeWidth: number;
    strokeColor: string;
}

export interface Marker {
    title: string;
    image: string;
    coordinates: LatLng;
}

export interface MapData {
    lines: Polyline[]
    markers: Marker[];
}

export interface Journey {
    name: string
    start: Location
    end: Location
}
