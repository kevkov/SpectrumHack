/* eslint-disable no-undef */
import React, { useEffect, useState } from 'react';
import { withScriptjs, withGoogleMap, GoogleMap, KmlLayer } from "react-google-maps"
import HeatmapLayer from 'react-google-maps/lib/components/visualization/HeatmapLayer';

const GetKmlUri = (showPollution, showSchools, startTime) => {  
  
  var kmlUrl = 'https://spectrummapapi.azurewebsites.net/api/map/1?showPollution=' +
                showPollution +
                '&showSchools=' +
                showSchools +
                '&startTime=' +
                startTime +
                '&startName=NorthGreenwich&startLongitude=0.00447&startLatitude=51.49847&endName=Westminster&endLongitude=-0.13563&endLatitude=51.4975'
                +
                '&rand=' + Math.random();

    return kmlUrl;
}

async function FetchPollutionData() {
  const url = 'https://spectrummapapi.azurewebsites.net/api/map/pollution';

  return fetch(url)
  .then(response => {
      if (!response.ok) {
          console.log(`*********** error calling endpoint ${url}: statusText: ${response.statusText}`);
          throw new Error(response.statusText)
      }
      return response.json()
  })
  .then(jsonData => {
    console.log('LAST THEN');
    console.log(jsonData.length);

    return jsonData.map((item) => {
      return {
        weight: item.weight,
        location: new google.maps.LatLng(item.location.latitude, item.location.longitude)
      }
    });
  });
}

async function GetHeatmapData() {
  const heatMapData = await FetchPollutionData();

  console.log('HM DATA');
  console.log(heatMapData);

  // const heatMapResult = heatMapData.map((item) => { 
  //   return new google.maps.LatLng(item.location.latitude, item.location.longitude)
  // });
  
//    heatMapData.map(function(v) {
//      return {
//         weight: v.weight,
//          location: new google.maps.LatLng(v.location.latitude, v.location.longitude)
//          }
//      });

  const heatMapResult = [
    {weight: 10000, location: new google.maps.LatLng(51.513329, -0.088950)},
    {weight: 10000, location: new google.maps.LatLng(51.513329, -0.088950)}
];

//     console.log('HMResult');
     console.log(heatMapResult);

    return heatMapResult;
}

const RouteMap = withScriptjs(withGoogleMap((props) =>   
{
  const [heatMapData, setHeatMapData] = useState(() => []);

  useEffect(async () => {
    const updatedHeaMapData = await FetchPollutionData();
    
    setHeatMapData(updatedHeaMapData);
  }, []);

  return <GoogleMap
    defaultZoom={12}
    defaultCenter={{ lat: 51.513329, lng: -0.088950 }}    
  >
    <KmlLayer
      url={GetKmlUri(props.showPollution, props.showSchools, props.startTime)}
      options={{ preserveViewport: true }}
    />

    <HeatmapLayer
    data={heatMapData}
    options={{dissapting: true, radius: 50}}
     />
  </GoogleMap>
}
))

export default RouteMap;